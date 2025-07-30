using ItTrAp.ManagementService.Application.Interfaces.Services;
using ItTrAp.ManagementService.Infrastructure.Events.InboundEvents;
using MediatR;

namespace ItTrAp.ManagementService.Infrastructure.EventHandlers.MovableItems;

public class MovableItemDeletedEventHandler(ILogger<MovableItemDeletedEventHandler> logger, IMovableItemService movableItemService) : INotificationHandler<MovableItemDeleted>
{
    public async Task Handle(MovableItemDeleted notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling MovableItemDeleted event for MovableItemId: {MovableItemId} at {Timestamp}", notification.MovableItemId, DateTime.UtcNow);

        await movableItemService.DeleteAsync(notification.MovableItemId, cancellationToken);
    }
}