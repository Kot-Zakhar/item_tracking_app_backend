using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.UserSessions;

public class EFUserSessionRepository(AppDbContext dbContext) : IUserSessionRepository
{

    public async Task<UserSession?> GetByIdAsync(uint id)
    {
        return await dbContext.UserSessions.FindAsync(id);
    }

    public async Task<UserSession?> GetByFingerprintAsync(string fingerprint)
    {
        return await dbContext.UserSessions
            .FirstOrDefaultAsync(x => x.Fingerprint == fingerprint);
    }

    public async Task<UserSession?> GetByRefreshTokenAsync(Guid refreshToken)
    {
        return await dbContext.UserSessions
            .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
    }

    public async Task<List<UserSession>> GetAllAsync()
    {
        return await dbContext.UserSessions.ToListAsync();
    }

    public async Task<UserSession> CreateAsync(UserSession userSession)
    {
        var entityEntry = await dbContext.UserSessions.AddAsync(userSession);
        return entityEntry.Entity;
    }

    public Task UpdateAsync(UserSession userSession)
    {
        dbContext.UserSessions.Update(userSession);
        return Task.CompletedTask;
    }

    public async Task<bool> DeleteAsync(uint id)
    {
        var userSession = await GetByIdAsync(id);
        if (userSession == null) return false;

        dbContext.UserSessions.Remove(userSession);
        return true;
    }

    public async Task DeleteByFingerprintAsync(string fingerprint)
    {
        var userSession = await GetByFingerprintAsync(fingerprint);
        if (userSession == null) return;

        dbContext.UserSessions.Remove(userSession);
    }

    public async Task DeleteExpiredAsync(uint userId)
    {
        var userSession = await dbContext.UserSessions
            .Where(x => x.User.Id == userId && x.ExpiresAt < DateTime.UtcNow)
            .ToListAsync();

        dbContext.UserSessions.RemoveRange(userSession);
    }

    public async Task DeleteByRefreshTokenAsync(Guid refreshToken)
    {
        var userSession = await GetByRefreshTokenAsync(refreshToken);
        if (userSession == null) return;

        dbContext.UserSessions.Remove(userSession);
    }
}