using ItTrAp.IdentityService.Interfaces.Persistence.Repositories;
using ItTrAp.IdentityService.Models;
using Microsoft.EntityFrameworkCore;

namespace ItTrAp.IdentityService.Persistence.Repositories;

public class EfUserRepository(AppDbContext dbContext) : EFRepository<User>(dbContext), IUserRepository
{
    private readonly AppDbContext dbContext = dbContext;

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
    }
}

