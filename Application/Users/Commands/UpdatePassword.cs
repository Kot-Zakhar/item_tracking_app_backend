using Application.Users.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Users.Commands;

public record UpdatePasswordCommand(int Id, string Password, string NewPassword, string NewPasswordConfirmation) : IRequest;

public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("User ID must be greater than 0.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Current password is required.");
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6).WithMessage("New password must be at least 6 characters long.");
        RuleFor(x => x.NewPasswordConfirmation).Equal(x => x.NewPassword).WithMessage("New password confirmation does not match.");
    }
}

public class UpdatePasswordHandler(IUserService userService) : IRequestHandler<UpdatePasswordCommand>
{
    public async Task Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        await userService.UpdatePasswordAsync(
            (uint)request.Id,
            request.Password,
            request.NewPassword,
            request.NewPasswordConfirmation);
    }
}