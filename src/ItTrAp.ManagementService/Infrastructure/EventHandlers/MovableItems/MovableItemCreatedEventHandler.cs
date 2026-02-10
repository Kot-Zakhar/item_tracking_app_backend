using ItTrAp.ManagementService.Application.Interfaces.Services;
using ItTrAp.ManagementService.Infrastructure.Events.InboundEvents;
using MediatR;

namespace ItTrAp.ManagementService.Infrastructure.EventHandlers.MovableItems;

public class MovableItemCreatedEventHandler(IMovableItemService movableItemService) : INotificationHandler<MovableItemCreated>
{
    public async Task Handle(MovableItemCreated notification, CancellationToken cancellationToken)
    {
        await movableItemService.CreateAsync(notification.MovableItemId, cancellationToken);
    }
}