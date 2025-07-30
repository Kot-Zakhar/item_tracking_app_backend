using ItTrAp.ManagementService.Application.Interfaces.Services;
using ItTrAp.ManagementService.Infrastructure.Events.InboundEvents.Locations;
using MediatR;

namespace ItTrAp.ManagementService.Infrastructure.EventHandlers.Locations;

public class LocationCreatedEventHandler(ILogger<LocationCreatedEventHandler> logger, ILocationService locationsService) : INotificationHandler<LocationCreated>
{
    public async Task Handle(LocationCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling LocationCreated event for LocationId: {LocationId} at {Timestamp}", notification.LocationId, DateTime.UtcNow);

        await locationsService.CreateAsync(notification.LocationId, notification.LocationCode, cancellationToken);
    }
}