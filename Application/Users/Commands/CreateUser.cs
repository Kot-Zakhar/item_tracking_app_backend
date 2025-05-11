using Application.Users.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Users.Commands;

public record CreateUserCommand(string FirstName, string LastName, string Phone, string Email, string Password, string PasswordConfirmation) : IRequest<uint>;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
        RuleFor(x => x.Phone).NotEmpty().Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number is required and must be in a valid international format.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Valid email is required.");
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        RuleFor(x => x.PasswordConfirmation).Equal(x => x.Password).WithMessage("Password confirmation does not match.");
    }
}

public class CreateUserHandler(IUserService userService) : IRequestHandler<CreateUserCommand, uint>
{
    public async Task<uint> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        return await userService.CreateUserAsync(
            request.FirstName,
            request.LastName,
            request.Phone,
            request.Email,
            request.Password,
            request.PasswordConfirmation);
    }
}