using ItTrAp.UserService.Application.Interfaces.Services;
using ItTrAp.UserService.Domain.Events.OutboundEvents;
using MediatR;

namespace ItTrAp.UserService.Application.EventHandlers;

public class UserDeletedEventHandler(ILogger<UserDeletedEventHandler> logger, IEventPublishingService eventPublisher) : INotificationHandler<UserDeleted>
{
    public async Task Handle(UserDeleted notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling outbound event of type {EventType} at {Timestamp}", nameof(UserDeleted), DateTime.UtcNow);
        await eventPublisher.PublishAsync(notification, cancellationToken);
    }
}