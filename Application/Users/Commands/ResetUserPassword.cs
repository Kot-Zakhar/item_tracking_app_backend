using Application.Users.DTOs;
using Application.Users.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Users.Commands;

public record ResetUserPasswordCommand(uint Id, ResetUserPasswordDto Passwords) : IRequest;

public class ResetUserPasswordCommandValidator : AbstractValidator<ResetUserPasswordCommand>
{
    public ResetUserPasswordCommandValidator()
    {
        RuleFor(x => (int)x.Id).GreaterThan(0).WithMessage("User ID must be greater than 0.");
        RuleFor(x => x.Passwords.NewPassword).NotEmpty().MinimumLength(6).WithMessage("New password must be at least 6 characters long.");
        RuleFor(x => x.Passwords.NewPasswordConfirmation).Equal(x => x.Passwords.NewPassword).WithMessage("New password confirmation does not match.");
    }
}

public class ResetUserPasswordHandler(IUserService userService) : IRequestHandler<ResetUserPasswordCommand>
{
    public async Task Handle(ResetUserPasswordCommand request, CancellationToken cancellationToken)
    {
        await userService.ResetPasswordAsync(request.Id, request.Passwords);
    }
}