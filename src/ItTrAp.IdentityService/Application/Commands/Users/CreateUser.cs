using ItTrAp.IdentityService.Application.Interfaces.Services;
using MediatR;

namespace ItTrAp.IdentityService.Application.Commands.Users;

public record CreateUserCommand(uint UserId, string Email): IRequest;

public class CreateUserCommandHandler(IUserService userService) : IRequestHandler<CreateUserCommand>
{
    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        request = request with { Email = request.Email.Trim().ToLowerInvariant() };
        await userService.CreateUserAsync(request.UserId, request.Email, cancellationToken);
    }
}