using ItTrAp.ManagementService.Application.Interfaces.Services;
using ItTrAp.ManagementService.Infrastructure.Events.InboundEvents.Locations;
using MediatR;

namespace ItTrAp.ManagementService.Infrastructure.EventHandlers.Locations;

public class LocationDeletedEventHandler(ILocationService locationsService) : INotificationHandler<LocationDeleted>
{
    public async Task Handle(LocationDeleted notification, CancellationToken cancellationToken)
    {
        await locationsService.DeleteAsync(notification.LocationId, cancellationToken);
    }
}