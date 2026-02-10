using Amazon.SQS;
using Amazon.SQS.Model;
using ItTrAp.IdentityService.Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace ItTrAp.IdentityService.Infrastructure.Workers;

public class SqsPoolingWorker(ILogger<SqsPoolingWorker> logger, IAmazonSQS sqsClient, IServiceProvider serviceProvider, IOptions<GlobalConfig> globalConfig) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(15000, stoppingToken); // TODO: Waiting for localstack to finish its configuration

        var receiveRequest = new ReceiveMessageRequest
        {
            QueueUrl = globalConfig.Value.SqsUrl,
            MaxNumberOfMessages = 10,
            WaitTimeSeconds = 20, // long-poll
            MessageAttributeNames = new List<string> { "All" },
            VisibilityTimeout = 300 // Add 5 minutes visibility timeout
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var response = await sqsClient.ReceiveMessageAsync(receiveRequest, stoppingToken);

                if (response.Messages == null || response.Messages.Count == 0)
                {
                    logger.LogDebug("No messages received from SQS.");
                    continue;
                }

                foreach (var msg in response.Messages)
                {
                    try
                    {
                        await ProcessMessageAsync(msg, stoppingToken);

                        var deleteResult = await sqsClient.DeleteMessageAsync(globalConfig.Value.SqsUrl, msg.ReceiptHandle, stoppingToken);
                        
                        logger.LogDebug("Delete result: {StatusCode} for message: {MessageId}", deleteResult.HttpStatusCode, msg.MessageId);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Failed to process message: {MessageId}", msg.MessageId);
                    }
                }
            }
            catch (TaskCanceledException)
            {
                logger.LogInformation("SQS polling was cancelled.");
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing SQS messages.");
            }

            await Task.Delay(5000);
        }
    }

    private async Task ProcessMessageAsync(Message msg, CancellationToken stoppingToken)
    {
        // Create a new scope for each message
        using (var scope = serviceProvider.CreateScope())
        {
            var processingService = scope.ServiceProvider.GetRequiredService<IInboundEventService>();
            await processingService.ProcessInboundEventAsync(msg.MessageId, msg.Body, stoppingToken);
        }
    }

}