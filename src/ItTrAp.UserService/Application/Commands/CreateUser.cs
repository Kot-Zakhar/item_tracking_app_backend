using ItTrAp.UserService.Application.DTOs;
using ItTrAp.UserService.Application.Interfaces;
using ItTrAp.Shared.Validators;
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
        RuleFor(x => x.User.Phone)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .PhoneNumber()
            .WithMessage("Phone number must be in a valid international format.");
        RuleFor(x => x.User.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
#pragma warning disable CS0618 // Type or member is obsolete
            .EmailAddress(FluentValidation.Validators.EmailValidationMode.Net4xRegex)
#pragma warning restore CS0618 // Type or member is obsolete
            .WithMessage("Valid email is required.");
        When(x => !string.IsNullOrEmpty(x.User.Avatar), () =>
        {
            RuleFor(x => x.User.Avatar).Must(url => Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
                .WithMessage("Avatar must be a valid URL.");
        });
    }
}

public class CreateUserHandler(IUserService userService) : IRequestHandler<CreateUserCommand, uint>
{
    public async Task<uint> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        request.User.Email = request.User.Email.Trim().ToLowerInvariant();;
        return await userService.CreateUserAsync(request.User);
    }
}