using ItTrAp.IdentityService.Infrastructure.Events.Users;
using ItTrAp.IdentityService.Application.Interfaces.Services;
using MediatR;

namespace ItTrAp.IdentityService.Infrastructure.EventHandlers.Users;

public class UserDeletedEventHandler(ILogger<UserDeletedEventHandler> logger, IUserService userService) : INotificationHandler<UserDeleted>
{
    public async Task Handle(UserDeleted notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling UserDeleted event for UserId: {UserId} at {Timestamp}", notification.UserId, DateTime.UtcNow);

        await userService.DeleteUserAsync(notification.UserId, cancellationToken);
    }
}