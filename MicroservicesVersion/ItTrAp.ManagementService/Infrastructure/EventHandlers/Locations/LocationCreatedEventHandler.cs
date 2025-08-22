using ItTrAp.ManagementService.Application.Interfaces.Services;
using ItTrAp.ManagementService.Infrastructure.Events.InboundEvents.Locations;
using MediatR;

namespace ItTrAp.ManagementService.Infrastructure.EventHandlers.Locations;

public class LocationCreatedEventHandler(ILocationService locationsService) : INotificationHandler<LocationCreated>
{
    public async Task Handle(LocationCreated notification, CancellationToken cancellationToken)
    {
        await locationsService.CreateAsync(notification.LocationId, cancellationToken);
    }
}