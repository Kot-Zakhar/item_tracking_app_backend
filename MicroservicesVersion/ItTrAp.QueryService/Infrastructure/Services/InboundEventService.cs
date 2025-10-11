using System.Text.Json;
using ItTrAp.QueryService.Infrastructure.Events;
using ItTrAp.QueryService.Infrastructure.Events.InboundEvents;
using ItTrAp.QueryService.Infrastructure.Events.OutboundEvents;
using ItTrAp.QueryService.Infrastructure.Models;
using MediatR;

namespace ItTrAp.QueryService.Infrastructure.Interfaces.Services;

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
            case nameof(MovableItemCreated):
                return JsonSerializer.Deserialize<MovableItemCreated>(jsonData, options);
            case nameof(MovableItemDeleted):
                return JsonSerializer.Deserialize<MovableItemDeleted>(jsonData, options);
            case nameof(LocationCreated):
                return JsonSerializer.Deserialize<LocationCreated>(jsonData, options);
            case nameof(LocationDeleted):
                return JsonSerializer.Deserialize<LocationDeleted>(jsonData, options);
            case nameof(UserCreated):
                return JsonSerializer.Deserialize<UserCreated>(jsonData, options);
            case nameof(UserDeleted):
                return JsonSerializer.Deserialize<UserDeleted>(jsonData, options);
            case nameof(MovableInstanceCreated):
                return JsonSerializer.Deserialize<MovableInstanceCreated>(jsonData, options);
            case nameof(MovableInstanceDeleted):
                return JsonSerializer.Deserialize<MovableInstanceDeleted>(jsonData, options);
            case nameof(MovableInstanceStatusChanged):
                return JsonSerializer.Deserialize<MovableInstanceStatusChanged>(jsonData, options);
            default:
                return null;
        }
    }
}