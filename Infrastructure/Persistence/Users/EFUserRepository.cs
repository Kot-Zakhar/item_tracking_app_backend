using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Users;

public class EfUserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task<User> CreateAsync(User user)
    {
        return (await dbContext.Users.AddAsync(user)).Entity;
    }

    public async Task<bool> DeleteAsync(uint id)
    {
        var user = await dbContext.Users.FindAsync(id);
        if (user == null)
        {
            return false;
        }

        dbContext.Users.Remove(user);

        return true;
    }

    public Task<List<User>> GetAllAsync()
    {
        return dbContext.Users.ToListAsync();
    }

    public Task<User?> GetByIdAsync(uint id)
    {
        return dbContext.Users.FindAsync(id).AsTask();
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        return dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }


    public Task UpdateAsync(User user)
    {
        dbContext.Users.Update(user);
        return Task.CompletedTask;
    }
}

