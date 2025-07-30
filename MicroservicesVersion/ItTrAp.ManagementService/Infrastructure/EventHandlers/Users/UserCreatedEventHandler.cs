using ItTrAp.ManagementService.Infrastructure.Events.OutboundEvents;
using ItTrAp.ManagementService.Application.Interfaces.Services;
using MediatR;

namespace ItTrAp.ManagementService.Infrastructure.EventHandlers.Users;

public class UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger, IUserService userService) : INotificationHandler<UserCreated>
{
    public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling UserCreated event for UserId: {UserId} at {Timestamp}", notification.UserId, DateTime.UtcNow);

        await userService.CreateAsync(notification.UserId, notification.UserEmail, cancellationToken);
    }
}