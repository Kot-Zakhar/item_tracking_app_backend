using Application.Users.DTOs;
using Application.Users.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Users.Commands;

public record UpdatePasswordCommand(int Id, UpdatePasswordDto Passwords) : IRequest;

public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("User ID must be greater than 0.");
        RuleFor(x => x.Passwords.Password).NotEmpty().WithMessage("Current password is required.");
        RuleFor(x => x.Passwords.NewPassword).NotEmpty().MinimumLength(6).WithMessage("New password must be at least 6 characters long.");
        RuleFor(x => x.Passwords.NewPasswordConfirmation).Equal(x => x.Passwords.NewPassword).WithMessage("New password confirmation does not match.");
    }
}

public class UpdatePasswordHandler(IUserService userService) : IRequestHandler<UpdatePasswordCommand>
{
    public async Task Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        await userService.UpdatePasswordAsync((uint)request.Id, request.Passwords);
    }
}