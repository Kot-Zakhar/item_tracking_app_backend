using Microsoft.EntityFrameworkCore;
using Domain.Aggregates.Users;
using Infrastructure.Interfaces.Persistence.Repositories;

namespace Infrastructure.EFPersistence.Repositories;

public class EfUserRepository(AppDbContext dbContext) : EFRepository<User>(dbContext), IUserRepository
{
    private readonly AppDbContext dbContext = dbContext;

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
    }
}

