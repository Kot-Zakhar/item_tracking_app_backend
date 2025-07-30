using System.Text.Json;
using ItTrAp.IdentityService.Infrastructure.Events;
using ItTrAp.IdentityService.Infrastructure.Events.Users;
using ItTrAp.IdentityService.Infrastructure.Models;
using MediatR;

namespace ItTrAp.IdentityService.Infrastructure.Interfaces.Services;

public class InboundEventService(IMediator mediator, ILogger<InboundEventService> logger) : IInboundEventService
{

    public async Task ProcessInboundEventAsync(string messageId, string eventData, CancellationToken stoppingToken)
    {
        // 1) Parse envelope
        var snsEnvelope = JsonSerializer.Deserialize<SnsNotification>(eventData);
        if (snsEnvelope == null)
        {
            logger.LogError("Failed to deserialize SNS envelope from message: {MessageId} {Body}", messageId, eventData);
            return;
        }

        logger.LogDebug("Processing message: {MessageId}\n{Body}", messageId, eventData);

        var envelope = JsonSerializer.Deserialize<JsonElement>(snsEnvelope.Message);

        // 2) Resolve concrete event type
        var @event = DeserializeEvent(envelope.GetProperty("type").GetString(), snsEnvelope.Message);

        if (@event == null)
        {
            logger.LogError("Failed to deserialize event from message: {MessageId} {Body}", messageId, eventData);
            return;
        }


        // 3) Dispatch via MediatR with scoped services
        await mediator.Publish(@event, stoppingToken);
    }


    private EventBase? DeserializeEvent(string? eventType, string? jsonData)
    {
        if (string.IsNullOrEmpty(eventType) || string.IsNullOrEmpty(jsonData))
        {
            return null;
        }

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        switch (eventType)
        {
            case nameof(UserCreated):
                return JsonSerializer.Deserialize<UserCreated>(jsonData, options);
            case nameof(UserDeleted):
                return JsonSerializer.Deserialize<UserDeleted>(jsonData, options);
            default:
                return null;
        }
    }
}