using ItTrAp.ManagementService.Application.Interfaces.Services;
using ItTrAp.ManagementService.Infrastructure.Events.InboundEvents.Locations;
using MediatR;

namespace ItTrAp.ManagementService.Infrastructure.EventHandlers.Locations;

public class LocationDeletedEventHandler(ILogger<LocationDeletedEventHandler> logger, ILocationService locationsService) : INotificationHandler<LocationDeleted>
{
    public async Task Handle(LocationDeleted notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling LocationDeleted event for LocationId: {LocationId} at {Timestamp}", notification.LocationId, DateTime.UtcNow);

        await locationsService.DeleteAsync(notification.LocationId, cancellationToken);
    }
}