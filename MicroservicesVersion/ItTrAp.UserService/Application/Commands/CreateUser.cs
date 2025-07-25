using ItTrAp.UserService.Application.DTOs;
using ItTrAp.UserService.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace ItTrAp.UserService.Application.Commands;

public record CreateUserCommand(CreateUserDto User) : IRequest<uint>;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.User.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(x => x.User.LastName).NotEmpty().WithMessage("Last name is required.");
        RuleFor(x => x.User.Phone).NotEmpty().Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Phone number is required and must be in a valid international format.");
        RuleFor(x => x.User.Email).NotEmpty().EmailAddress().WithMessage("Valid email is required.");
        RuleFor(x => x.User.Avatar).Must(url => string.IsNullOrEmpty(url) || Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            .WithMessage("Avatar must be a valid URL.");
    }
}

public class CreateUserHandler(IUserService userService) : IRequestHandler<CreateUserCommand, uint>
{
    public async Task<uint> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        return await userService.CreateUserAsync(request.User);
    }
}