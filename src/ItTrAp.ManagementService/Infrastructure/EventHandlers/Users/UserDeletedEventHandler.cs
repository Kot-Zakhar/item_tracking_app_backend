using ItTrAp.ManagementService.Infrastructure.Events.OutboundEvents;
using ItTrAp.ManagementService.Application.Interfaces.Services;
using MediatR;

namespace ItTrAp.ManagementService.Infrastructure.EventHandlers.Users;

public class UserDeletedEventHandler(IUserService userService) : INotificationHandler<UserDeleted>
{
    public async Task Handle(UserDeleted notification, CancellationToken cancellationToken)
    {
        await userService.DeleteAsync(notification.UserId, cancellationToken);
    }
}