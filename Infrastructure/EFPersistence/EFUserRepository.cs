using Domain.Models;
using Abstractions.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EFPersistence;

public class EfUserRepository(AppDbContext dbContext) : EFRepository<User>(dbContext), IUserRepository
{
    private readonly AppDbContext dbContext = dbContext;

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
    }
}

