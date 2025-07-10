using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.AWS;

public static class AwsServiceExtensions
{
    public static IServiceCollection AddAwsServices(this IServiceCollection services, IConfiguration configuration)
    {
        var isLocal = configuration.GetValue<bool>("AWS:UseLocalStack");
        
        if (isLocal)
        {
            // LocalStack configuration for development
            services.AddLocalStackServices(configuration);
        }
        else
        {
            // Real AWS services for production
            services.AddAWSService<IAmazonSimpleNotificationService>();
            services.AddAWSService<IAmazonSQS>();
        }

        // Register event bus
        services.AddSingleton<IEventBus, AwsEventBus>();
        
        // Register background service for message processing
        services.AddHostedService<AwsMessageProcessorService>();

        return services;
    }

    private static IServiceCollection AddLocalStackServices(this IServiceCollection services, IConfiguration configuration)
    {
        var endpoint = configuration["AWS:LocalStack:Endpoint"] ?? "http://localhost:4566";
        
        var snsConfig = new Amazon.SimpleNotificationService.AmazonSimpleNotificationServiceConfig
        {
            ServiceURL = endpoint,
            UseHttp = true,
            AuthenticationRegion = "us-east-1"
        };
        
        var sqsConfig = new Amazon.SQS.AmazonSQSConfig
        {
            ServiceURL = endpoint,
            UseHttp = true,
            AuthenticationRegion = "us-east-1"
        };

        services.AddSingleton<IAmazonSimpleNotificationService>(provider =>
            new Amazon.SimpleNotificationService.AmazonSimpleNotificationServiceClient(
                "test", "test", snsConfig));

        services.AddSingleton<IAmazonSQS>(provider =>
            new Amazon.SQS.AmazonSQSClient("test", "test", sqsConfig));

        return services;
    }
}

public class AwsMessageProcessorService : BackgroundService
{
    private readonly IEventBus _eventBus;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AwsMessageProcessorService> _logger;

    public AwsMessageProcessorService(
        IEventBus eventBus,
        IServiceProvider serviceProvider,
        ILogger<AwsMessageProcessorService> logger)
    {
        _eventBus = eventBus;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Subscribe to relevant events for this service
        await SetupEventSubscriptions();
        
        _logger.LogInformation("AWS Message Processor Service started");

        // Keep the service running
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task SetupEventSubscriptions()
    {
        // Subscribe to events from other services
        await _eventBus.SubscribeAsync<UserDeletedEvent>(async @event =>
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Publish(@event);
        });

        await _eventBus.SubscribeAsync<ItemDeletedEvent>(async @event =>
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Publish(@event);
        });

        // Add more subscriptions as needed
    }
}
