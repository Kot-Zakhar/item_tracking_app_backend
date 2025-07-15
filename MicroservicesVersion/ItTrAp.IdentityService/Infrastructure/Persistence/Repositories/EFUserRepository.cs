using ItTrAp.IdentityService.Infrastructure.Interfaces.Persistence.Repositories;
using ItTrAp.IdentityService.Domain;
using Microsoft.EntityFrameworkCore;

namespace ItTrAp.IdentityService.Infrastructure.Persistence.Repositories;

public class EfUserRepository(AppDbContext dbContext) : EFRepository<User>(dbContext), IUserRepository
{
    private readonly AppDbContext dbContext = dbContext;

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
    }
}

