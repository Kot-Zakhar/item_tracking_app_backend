using Application.Users.DTOs;
using Application.Users.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Users.Commands;

public record UpdateUserCommand(int Id, UpdateUserDto User) : IRequest;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("User ID must be greater than 0.");
        RuleFor(x => x.User.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone must be in a valid international format.");
    }
}

public class UpdateUserHandler(IUserService userService) : IRequestHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        await userService.UpdateUserAsync((uint)request.Id, request.User);
    }
}