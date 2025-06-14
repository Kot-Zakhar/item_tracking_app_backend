using Application.Auth.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Auth.Commands;

public record SignOutCommand(string RefreshToken) : IRequest;

public class SignOutCommandValidator : AbstractValidator<SignOutCommand>
{
    public SignOutCommandValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("Refresh token is required.");
    }
}

public class SignOutHandler(IAuthenticationService authService) : IRequestHandler<SignOutCommand>
{
    public async Task Handle(SignOutCommand command, CancellationToken cancellationToken)
    {
        await authService.SignOutAsync(command.RefreshToken);
    }
}