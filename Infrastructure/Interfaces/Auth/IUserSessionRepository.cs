using Domain.Users;

namespace Infrastructure.Interfaces.Auth;

public interface IUserSessionRepository
{
    Task<UserSession?> GetByIdAsync(uint id);
    Task<UserSession?> GetByFingerprintAsync(string fingerprint);
    Task<UserSession?> GetByRefreshTokenAsync(Guid refreshToken);
    Task<List<UserSession>> GetAllAsync();
    Task<UserSession> CreateAsync(UserSession userSession);
    Task UpdateAsync(UserSession userSession);
    Task<bool> DeleteAsync(uint id);
    Task DeleteByFingerprintAsync(string fingerprint);
    Task DeleteExpiredAsync(uint userId);
    Task DeleteByRefreshTokenAsync(Guid refreshToken);
}
