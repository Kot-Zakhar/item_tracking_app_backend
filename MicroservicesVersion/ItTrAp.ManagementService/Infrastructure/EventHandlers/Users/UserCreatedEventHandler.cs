using ItTrAp.ManagementService.Infrastructure.Events.OutboundEvents;
using ItTrAp.ManagementService.Application.Interfaces.Services;
using MediatR;

namespace ItTrAp.ManagementService.Infrastructure.EventHandlers.Users;

public class UserCreatedEventHandler(IUserService userService) : INotificationHandler<UserCreated>
{
    public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        await userService.CreateAsync(notification.UserId, notification.UserEmail, cancellationToken);
    }
}