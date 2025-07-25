using ItTrAp.LocationService.Application.Interfaces.Services;
using ItTrAp.LocationService.Domain.Events.Locations;
using MediatR;

namespace ItTrAp.LocationService.Application.EventHandlers;

public class LocationCreatedEventHandler(ILogger<LocationCreatedEventHandler> logger, IEventPublishingService eventPublisher) : INotificationHandler<LocationCreated>
{
    private readonly IEventPublishingService _eventPublisher = eventPublisher;

    public async Task Handle(LocationCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling outbound event of type {EventType} at {Timestamp}", nameof(LocationCreated), DateTime.UtcNow);
        await _eventPublisher.PublishAsync(notification, cancellationToken);
    }
}