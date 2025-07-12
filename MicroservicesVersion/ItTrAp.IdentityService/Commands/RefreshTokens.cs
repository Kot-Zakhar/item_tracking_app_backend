using FluentValidation;
using ItTrAp.IdentityService.Interfaces.Services;
using MediatR;

namespace ItTrAp.IdentityService.Commands;

public record RefreshTokenCommand(string RefreshToken, string Fingerprint) : IRequest<SignInResponse>;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("Refresh token is required.");
        RuleFor(x => x.Fingerprint).NotEmpty().WithMessage("Fingerprint is required.");
    }
}

public class RefreshTokenHandler(IAuthenticationService authService) : IRequestHandler<RefreshTokenCommand, SignInResponse>
{
    public async Task<SignInResponse> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var (accessToken, refreshToken, expiresAt) = await authService.RefreshTokensAsync(command.RefreshToken, command.Fingerprint);
        return new SignInResponse(accessToken, refreshToken, expiresAt);
    }
}