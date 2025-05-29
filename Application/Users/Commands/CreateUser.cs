using Application.Users.DTOs;
using Application.Users.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Users.Commands;

public record CreateUserCommand(CreateUserDto User) : IRequest<uint>;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.User.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(x => x.User.LastName).NotEmpty().WithMessage("Last name is required.");
        RuleFor(x => x.User.Phone).NotEmpty().Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number is required and must be in a valid international format.");
        RuleFor(x => x.User.Email).NotEmpty().EmailAddress().WithMessage("Valid email is required.");
        RuleFor(x => x.User.Password).NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        RuleFor(x => x.User.PasswordConfirmation).Equal(x => x.User.Password).WithMessage("Password confirmation does not match.");
    }
}

public class CreateUserHandler(IUserService userService) : IRequestHandler<CreateUserCommand, uint>
{
    public async Task<uint> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        return await userService.CreateUserAsync(request.User);
    }
}