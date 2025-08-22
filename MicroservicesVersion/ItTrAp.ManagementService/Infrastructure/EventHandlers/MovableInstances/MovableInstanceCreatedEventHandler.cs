using ItTrAp.ManagementService.Application.Interfaces.Services;
using ItTrAp.ManagementService.Infrastructure.Events.InboundEvents.MovableInstances;
using MediatR;

namespace ItTrAp.ManagementService.Infrastructure.EventHandlers.MovableInstances;

public class MovableInstanceCreatedEventHandler(IMovableInstanceService service) : INotificationHandler<MovableInstanceCreated>
{
    public async Task Handle(MovableInstanceCreated notification, CancellationToken ct)
    {
        await service.CreateAsync(notification.MovableItemId, notification.MovableInstanceId, ct);
    }
}
