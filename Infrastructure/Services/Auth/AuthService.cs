using Application.Auth.Dtos;
using Application.Auth.Interfaces;
using Application.Common.Interfaces;
using Application.Users.Interfaces;
using Domain.Users;
using Infrastructure.Interfaces.Auth;

namespace Infrastructure.Services.Auth;

// TODO: the actual use of userAgent

public class AuthService(
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    IUserRepository userRepository,
    IUserSessionRepository sessionRepository,
    IUnitOfWork unitOfWork
) : IAuthService
{
    public async Task<SessionInfoDto> SignInAsync(string email, string password, string fingerprint, string userAgent)
    {
        var user = await userRepository.GetByEmailAsync(email);

        if (user == null)
        {
            throw new Exception("Wrong email or password");
        }

        var authData = user.GetAuthenticationData();
        if (!passwordHasher.VerifyPassword(password, authData.Hash, authData.Salt))
        {
            throw new Exception("Wrong email or password");
        }

        await sessionRepository.DeleteByFingerprintAsync(fingerprint);
        await sessionRepository.DeleteExpiredAsync(user.Id);

        var session = UserSession.Create(user, fingerprint, userAgent);
        session = await sessionRepository.CreateAsync(session);

        var success = await unitOfWork.SaveChangesAsync();
        if (session.Id == 0 || !success)
        {
            throw new Exception("Failed to create session");
        }

        var accessToken = jwtTokenGenerator.GenerateAccessToken(user, session);

        return new SessionInfoDto(accessToken, session.RefreshToken.ToString(), session.ExpiresAt);
    }

    public async Task SignOutAsync(string refreshToken)
    {
        var parsedToken = Guid.TryParse(refreshToken, out var token);
        if (!parsedToken)
        {
            throw new Exception("Invalid refresh token");
        }

        await sessionRepository.DeleteByRefreshTokenAsync(token);
        var success = await unitOfWork.SaveChangesAsync();
        if (!success)
        {
            throw new Exception("Failed to delete session");
        }
    }

    // TODO: proper use of fingerprint: when not matching - sign out, as this is a potential attack
    public async Task<SessionInfoDto> RefreshTokensAsync(string refreshToken, string fingerprint)
    {
        var parsedToken = Guid.TryParse(refreshToken, out var token);
        if (!parsedToken)
        {
            throw new Exception("Invalid refresh token");
        }
        
        var session = await sessionRepository.GetByRefreshTokenAsync(token);
        if (session == null)
        {
            throw new Exception("Session not found");
        }

        if (session.IsExpired())
        {
            throw new Exception("Session expired");
        }

        var user = session.User;
        await sessionRepository.DeleteByFingerprintAsync(fingerprint);
        await sessionRepository.DeleteExpiredAsync(user.Id);

        var newSession = UserSession.Create(user, fingerprint, session.UserAgent);
        newSession = await sessionRepository.CreateAsync(newSession);

        var success = await unitOfWork.SaveChangesAsync();
        if (newSession.Id == 0 || !success)
        {
            throw new Exception("Failed to create session");
        }

        var accessToken = jwtTokenGenerator.GenerateAccessToken(user, newSession);

        return new SessionInfoDto(accessToken, newSession.RefreshToken.ToString(), newSession.ExpiresAt);
    }
}
