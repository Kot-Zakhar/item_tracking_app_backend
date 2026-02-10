using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using ItTrAp.InventoryService.Infrastructure.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Options;

namespace ItTrAp.InventoryService.Infrastructure.Services;

public class SnsEventPublishingService : IEventPublishingService
{
    private readonly IAmazonSimpleNotificationService _snsClient;
    private readonly string _topicArn;

    public SnsEventPublishingService(IAmazonSimpleNotificationService snsClient, IOptions<GlobalConfig> config)
    {
        _snsClient = snsClient;
        _topicArn = config.Value.OutboundSnsTopicArn;
    }

    public async Task PublishAsync<TEvent>(TEvent eventMessage, CancellationToken cancellationToken = default) where TEvent : INotification
    {
        var messageBody = JsonSerializer.Serialize(eventMessage, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        var sendMessageRequest = new PublishRequest
        {
            TopicArn = _topicArn,
            Message = messageBody
        };

        await _snsClient.PublishAsync(sendMessageRequest, cancellationToken);
    }
}