using Application.Auth.Dtos;

namespace Application.Auth.Interfaces;

public interface IAuthService
{
    Task<SessionInfoDto> SignInAsync(string email, string password, string fingerprint, string userAgent);
    Task SignOutAsync(string refreshToken);
    Task<SessionInfoDto> RefreshTokensAsync(string refreshToken, string fingerprint);
}