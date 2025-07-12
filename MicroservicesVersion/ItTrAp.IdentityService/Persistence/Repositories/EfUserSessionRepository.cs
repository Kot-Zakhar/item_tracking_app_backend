using ItTrAp.IdentityService.Interfaces.Persistence.Repositories;
using ItTrAp.IdentityService.Models;
using Microsoft.EntityFrameworkCore;

namespace ItTrAp.IdentityService.Persistence.Repositories;

public class EFUserSessionRepository(AppDbContext dbContext) : EFRepository<UserSession>(dbContext), IUserSessionRepository
{
    private readonly AppDbContext dbContext = dbContext;

    public async Task<UserSession?> GetByFingerprintAsync(string fingerprint, CancellationToken ct = default)
    {
        return await dbContext.UserSessions
            .FirstOrDefaultAsync(x => x.Fingerprint == fingerprint);
    }

    public async Task<UserSession?> GetByRefreshTokenAsync(Guid refreshToken, CancellationToken ct = default)
    {
        return await dbContext.UserSessions
            .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
    }

    public async Task DeleteByFingerprintAsync(string fingerprint, CancellationToken ct = default)
    {
        var userSession = await GetByFingerprintAsync(fingerprint);
        if (userSession == null) return;

        dbContext.UserSessions.Remove(userSession);
    }

    public async Task DeleteExpiredAsync(uint userId, CancellationToken ct = default)
    {
        var userSession = await dbContext.UserSessions
            .Where(x => x.User.Id == userId && x.ExpiresAt < DateTime.UtcNow)
            .ToListAsync();

        dbContext.UserSessions.RemoveRange(userSession);
    }

    public async Task DeleteByRefreshTokenAsync(Guid refreshToken, CancellationToken ct = default)
    {
        var userSession = await GetByRefreshTokenAsync(refreshToken);
        if (userSession == null) return;

        dbContext.UserSessions.Remove(userSession);
    }
}