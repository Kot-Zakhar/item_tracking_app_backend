using Domain.Models;

namespace Abstractions.Auth;

public interface IUserSessionRepository : IRepository<UserSession>
{
    Task<UserSession?> GetByFingerprintAsync(string fingerprint, CancellationToken ct = default);
    Task<UserSession?> GetByRefreshTokenAsync(Guid refreshToken, CancellationToken ct = default);
    Task DeleteByFingerprintAsync(string fingerprint, CancellationToken ct = default);
    Task DeleteExpiredAsync(uint userId, CancellationToken ct = default);
    Task DeleteByRefreshTokenAsync(Guid refreshToken, CancellationToken ct = default);
}
