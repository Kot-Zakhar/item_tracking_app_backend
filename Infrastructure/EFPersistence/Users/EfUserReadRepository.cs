using Application.Common.DTOs;
using Application.Users.Interfaces;
using Domain.Users.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EFPersistence.Users;

// TODO: Fix Avatar property in UserViewModel

public class EfUserReadRepository : IUserReadRepository, IUserUniquenessChecker
{
    private readonly AppDbContext _dbContext;

    public EfUserReadRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserDto?> GetByIdAsync(uint id, CancellationToken ct = default)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Where(user => user.Id == id)
            .Select(user => new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Email = user.Email,
                // Avatar = user.Avatar
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<UserDto>> GetAllFiltered(string? search, int? top, CancellationToken ct = default)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Where(user => string.IsNullOrEmpty(search) || user.FirstName.Contains(search) || user.LastName.Contains(search))
            .Select(user => new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Email = user.Email,
                // Avatar = user.Avatar
            })
            .Take(top ?? 10)
            .ToListAsync(ct);
    }

    public Task<bool> IsEmailUniqueAsync(string email, CancellationToken ct = default)
    {
        return _dbContext.Users
            .AsNoTracking()
            .AllAsync(user => user.Email != email, ct);
    }

    public Task<bool> IsPhoneUniqueAsync(string phone, CancellationToken ct = default)
    {
        return _dbContext.Users
            .AsNoTracking()
            .AllAsync(user => user.Phone != phone, ct);
    }
}
