using ItTrAp.UserService.Application.DTOs;
using ItTrAp.UserService.Application.Interfaces;
using ItTrAp.Shared.Validators;
using FluentValidation;
using MediatR;

namespace ItTrAp.UserService.Application.Commands;

public record UpdateUserCommand(uint Id, UpdateUserDto User) : IRequest;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => (int)x.Id).GreaterThan(0).WithMessage("User ID must be greater than 0.");
        RuleFor(x => x.User.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(x => x.User.LastName).NotEmpty().WithMessage("Last name is required.");
        RuleFor(x => x.User.Phone)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .PhoneNumber()
            .WithMessage("Phone must be in a valid international format.");
        When(x => x.User.Avatar != null, () =>
        {
            RuleFor(x => x.User.Avatar).Must(url => Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
                .WithMessage("Avatar must be a valid URL.");
        });
    }
}

public class UpdateUserHandler(IUserService userService) : IRequestHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        await userService.UpdateUserAsync(request.Id, request.User);
    }
}