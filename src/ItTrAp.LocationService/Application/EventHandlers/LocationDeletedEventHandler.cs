using ItTrAp.LocationService.Application.Interfaces.Services;
using ItTrAp.LocationService.Domain.Events.Locations;
using MediatR;

namespace ItTrAp.LocationService.Application.EventHandlers;

public class LocationDeletedEventHandler(ILogger<LocationDeletedEventHandler> logger, IEventPublishingService eventPublisher) : INotificationHandler<LocationDeleted>
{
    private readonly IEventPublishingService _eventPublisher = eventPublisher;

    public async Task Handle(LocationDeleted notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling outbound event of type {EventType} at {Timestamp}", nameof(LocationDeleted), DateTime.UtcNow);
        await _eventPublisher.PublishAsync(notification, cancellationToken);
    }
}