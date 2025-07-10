using Application.Auth.DTOs;

namespace Application.Auth.Interfaces;

public interface IAuthenticationService
{
    Task<SessionInfoDto> SignInAsync(string email, string password, string fingerprint, string userAgent);
    Task SignOutAsync(string refreshToken);
    Task<SessionInfoDto> RefreshTokensAsync(string refreshToken, string fingerprint);
}