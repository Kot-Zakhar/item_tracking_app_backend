using Application.Users.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Users.Commands;

public record DeleteUserCommand(uint Id) : IRequest;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => (int)x.Id).GreaterThan(0).WithMessage("User ID must be greater than 0.");
    }
}

public class DeleteUserHandler(IUserService userService) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await userService.DeleteUserAsync(request.Id);
    }
}