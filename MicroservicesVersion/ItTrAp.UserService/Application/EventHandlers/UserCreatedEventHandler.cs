using ItTrAp.UserService.Application.Interfaces.Services;
using ItTrAp.UserService.Domain.Events.OutboundEvents;
using MediatR;

namespace ItTrAp.UserService.Application.EventHandlers;

public class UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger, IEventPublishingService eventPublisher) : INotificationHandler<UserCreated>
{
    public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling outbound event of type {EventType} at {Timestamp}", nameof(UserCreated), DateTime.UtcNow);
        await eventPublisher.PublishAsync(notification, cancellationToken);
    }
}