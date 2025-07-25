using ItTrAp.InventoryService.Application.Interfaces.Services;
using ItTrAp.InventoryService.Domain.Events.MovableItems;
using MediatR;

namespace ItTrAp.InventoryService.Application.EventHandlers.MovableItems;

public class MovableItemCreatedEventHandler(ILogger<MovableItemCreatedEventHandler> logger, IEventPublishingService eventPublisher) : INotificationHandler<MovableItemCreated>
{
    private readonly IEventPublishingService _eventPublisher = eventPublisher;

    public async Task Handle(MovableItemCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling outbound event of type {EventType} at {Timestamp}", nameof(MovableItemCreated), DateTime.UtcNow);
        await _eventPublisher.PublishAsync(notification, cancellationToken);
    }
}