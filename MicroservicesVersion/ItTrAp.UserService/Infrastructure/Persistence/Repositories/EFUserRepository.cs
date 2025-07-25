using Microsoft.EntityFrameworkCore;
using ItTrAp.UserService.Domain.Aggregates;
using ItTrAp.UserService.Infrastructure.Interfaces.Persistence.Repositories;

namespace ItTrAp.UserService.Infrastructure.Persistence.Repositories;

public class EfUserRepository(AppDbContext dbContext) : EFRepository<User, uint>(dbContext), IUserRepository
{
    private readonly AppDbContext dbContext = dbContext;

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
    }
}

