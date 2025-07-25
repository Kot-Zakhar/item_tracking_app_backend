using ItTrAp.InventoryService.Application.Interfaces.Services;
using ItTrAp.InventoryService.Domain.Events.MovableItems;
using MediatR;

namespace ItTrAp.InventoryService.Application.EventHandlers.MovableItems;

public class MovableItemDeletedEventHandler(ILogger<MovableItemDeletedEventHandler> logger, IEventPublishingService eventPublisher) : INotificationHandler<MovableItemDeleted>
{
    private readonly IEventPublishingService _eventPublisher = eventPublisher;

    public async Task Handle(MovableItemDeleted notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling outbound event of type {EventType} at {Timestamp}", nameof(MovableItemDeleted), DateTime.UtcNow);
        await _eventPublisher.PublishAsync(notification, cancellationToken);
    }
}