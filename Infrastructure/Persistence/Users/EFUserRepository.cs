using Domain.Users;
using Infrastructure.Interfaces.Users;
using Infrastructure.Persistence.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Users;

public class EfUserRepository(AppDbContext dbContext) : EFRepository<User>(dbContext), IUserRepository
{
    private readonly AppDbContext dbContext = dbContext;

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
    }
}

