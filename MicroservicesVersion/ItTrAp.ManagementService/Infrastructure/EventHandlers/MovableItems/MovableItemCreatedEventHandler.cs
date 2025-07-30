using ItTrAp.ManagementService.Application.Interfaces.Services;
using ItTrAp.ManagementService.Infrastructure.Events.InboundEvents;
using MediatR;

namespace ItTrAp.ManagementService.Infrastructure.EventHandlers.MovableItems;

public class MovableItemCreatedEventHandler(ILogger<MovableItemCreatedEventHandler> logger, IMovableItemService movableItemService) : INotificationHandler<MovableItemCreated>
{
    public async Task Handle(MovableItemCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling MovableItemCreated event for MovableItemId: {MovableItemId} at {Timestamp}", notification.MovableItemId, DateTime.UtcNow);

        await movableItemService.CreateAsync(notification.MovableItemId, cancellationToken);
    }
}