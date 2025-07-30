using ItTrAp.ManagementService.Application.Interfaces.Services;
using MediatR;

namespace ItTrAp.ManagementService.Application.Commands.Users;

public record DeleteUserCommand(uint Id) : IRequest;

public class DeleteUserCommandHandler(IUserService userService) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await userService.DeleteAsync(request.Id, cancellationToken);
    }
}