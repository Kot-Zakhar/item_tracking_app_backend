using ItTrAp.ManagementService.Application.Interfaces.Services;
using ItTrAp.ManagementService.Infrastructure.Events.InboundEvents.MovableInstances;
using MediatR;

namespace ItTrAp.ManagementService.Infrastructure.EventHandlers.MovableInstances;

public class MovableInstanceDeletedEventHandler(IMovableInstanceService service) : INotificationHandler<MovableInstanceDeleted>
{
    public async Task Handle(MovableInstanceDeleted notification, CancellationToken ct)
    {
        await service.DeleteAsync(notification.MovableItemId, notification.MovableInstanceId, ct);
    }
}
