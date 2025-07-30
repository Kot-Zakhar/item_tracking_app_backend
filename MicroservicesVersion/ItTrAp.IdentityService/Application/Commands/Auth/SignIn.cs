using FluentValidation;
using ItTrAp.IdentityService.Application.Interfaces.Services;
using MediatR;

namespace ItTrAp.IdentityService.Application.Commands.Auth;

public record SignInResponse(string AccessToken, string RefreshToken, DateTime ExpiresAt);

public record SignInCommand(string Email, string Password, string Fingerprint, string UserAgent) : IRequest<SignInResponse>;

public class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Valid email is required.");
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        RuleFor(x => x.Fingerprint).NotEmpty().WithMessage("Fingerprint is required.");
        RuleFor(x => x.UserAgent).NotEmpty().WithMessage("User agent is required.");
    }
}

public class SignInHandler(IAuthenticationService authService) : IRequestHandler<SignInCommand, SignInResponse>
{
    public async Task<SignInResponse> Handle(SignInCommand command, CancellationToken cancellationToken)
    {
        var (accessToken, refreshToken, expiresAt) = await authService.SignInAsync(command.Email, command.Password, command.Fingerprint, command.UserAgent);
        return new SignInResponse(accessToken, refreshToken, expiresAt);
    }
}