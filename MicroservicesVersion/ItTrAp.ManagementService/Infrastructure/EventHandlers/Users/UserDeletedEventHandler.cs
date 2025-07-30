using ItTrAp.ManagementService.Infrastructure.Events.OutboundEvents;
using ItTrAp.ManagementService.Application.Interfaces.Services;
using MediatR;

namespace ItTrAp.ManagementService.Infrastructure.EventHandlers.Users;

public class UserDeletedEventHandler(ILogger<UserDeletedEventHandler> logger, IUserService userService) : INotificationHandler<UserDeleted>
{
    public async Task Handle(UserDeleted notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling UserDeleted event for UserId: {UserId} at {Timestamp}", notification.UserId, DateTime.UtcNow);

        await userService.DeleteAsync(notification.UserId, cancellationToken);
    }
}