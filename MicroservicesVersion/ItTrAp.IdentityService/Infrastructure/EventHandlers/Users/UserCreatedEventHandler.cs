using ItTrAp.IdentityService.Infrastructure.Events.Users;
using ItTrAp.IdentityService.Application.Interfaces.Services;
using MediatR;

namespace ItTrAp.IdentityService.Infrastructure.EventHandlers.Users;

public class UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger, IUserService userService) : INotificationHandler<UserCreated>
{
    public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling UserCreated event for UserId: {UserId} at {Timestamp}", notification.UserId, DateTime.UtcNow);

        await userService.CreateUserAsync(notification.UserId, notification.UserEmail, cancellationToken);
    }
}