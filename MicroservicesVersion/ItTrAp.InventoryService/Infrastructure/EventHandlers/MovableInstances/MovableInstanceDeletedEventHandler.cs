using ItTrAp.InventoryService.Infrastructure.Interfaces.Services;
using ItTrAp.InventoryService.Domain.Events.MovableInstances;
using MediatR;

namespace ItTrAp.InventoryService.Infrastructure.EventHandlers.MovableInstances;

public class MovableInstanceDeletedEventHandler(ILogger<MovableInstanceDeletedEventHandler> logger, IEventPublishingService eventPublisher) : INotificationHandler<MovableInstanceDeleted>
{
    private readonly IEventPublishingService _eventPublisher = eventPublisher;

    public async Task Handle(MovableInstanceDeleted notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling outbound event of type {EventType} at {Timestamp}", nameof(MovableInstanceDeleted), DateTime.UtcNow);
        await _eventPublisher.PublishAsync(notification, cancellationToken);
    }
}