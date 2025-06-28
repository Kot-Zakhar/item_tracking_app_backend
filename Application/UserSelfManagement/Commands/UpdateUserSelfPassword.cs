using Application.UserSelfManagement.DTOs;
using Application.UserSelfManagement.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.UserSelfManagement.Commands;

public record UpdateUserSelfPasswordCommand(uint Id, UpdateUserSelfPasswordDto Passwords) : IRequest;

public class UpdateUserSelfPasswordCommandValidator : AbstractValidator<UpdateUserSelfPasswordCommand>
{
    public UpdateUserSelfPasswordCommandValidator()
    {
        RuleFor(x => (int)x.Id).GreaterThan(0).WithMessage("User ID must be greater than 0.");
        RuleFor(x => x.Passwords.Password).NotEmpty().WithMessage("Current password is required.");
        RuleFor(x => x.Passwords.NewPassword).NotEmpty().MinimumLength(6).WithMessage("New password must be at least 6 characters long.");
        RuleFor(x => x.Passwords.NewPasswordConfirmation).Equal(x => x.Passwords.NewPassword).WithMessage("New password confirmation does not match.");
    }
}

public class UpdateUserSelfPasswordHandler(IUserSelfManagementService userService) : IRequestHandler<UpdateUserSelfPasswordCommand>
{
    public async Task Handle(UpdateUserSelfPasswordCommand request, CancellationToken cancellationToken)
    {
        await userService.UpdateUserSelfPasswordAsync(request.Id, request.Passwords);
    }
}