using ItTrAp.InventoryService.Infrastructure.Interfaces.Services;
using ItTrAp.InventoryService.Domain.Events.MovableInstances;
using MediatR;

namespace ItTrAp.InventoryService.Infrastructure.EventHandlers.MovableInstances;

public class MovableInstanceCreatedEventHandler(ILogger<MovableInstanceCreatedEventHandler> logger, IEventPublishingService eventPublisher) : INotificationHandler<MovableInstanceCreated>
{
    private readonly IEventPublishingService _eventPublisher = eventPublisher;

    public async Task Handle(MovableInstanceCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling outbound event of type {EventType} at {Timestamp}", nameof(MovableInstanceCreated), DateTime.UtcNow);
        await _eventPublisher.PublishAsync(notification, cancellationToken);
    }
}