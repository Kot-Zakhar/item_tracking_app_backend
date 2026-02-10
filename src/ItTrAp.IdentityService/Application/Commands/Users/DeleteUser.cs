using ItTrAp.IdentityService.Application.Interfaces.Services;
using MediatR;

namespace ItTrAp.IdentityService.Application.Commands.Users;

public record DeleteUserCommand(uint UserId) : IRequest;

public class DeleteUserCommandHandler(IUserService userService) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await userService.DeleteUserAsync(request.UserId, cancellationToken);
    }
}