using ItTrAp.UserService.Application.DTOs;
using ItTrAp.UserService.Application.Interfaces;
using ItTrAp.UserService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ItTrAp.UserService.Infrastructure.Persistence.Repositories;

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
                Avatar = user.Avatar
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<UserDto>> GetByIdsAsync(List<uint> ids, CancellationToken ct = default)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Where(user => ids.Contains(user.Id))
            .Select(user => new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Email = user.Email,
                Avatar = user.Avatar
            })
            .ToListAsync(ct);
    }

    public async Task<List<UserDto>> GetAllFiltered(string? search, int? top, CancellationToken ct = default)
    {
        var query = _dbContext.Users
            .AsNoTracking()
            .Where(user => string.IsNullOrEmpty(search) || user.FirstName.Contains(search) || user.LastName.Contains(search))
            .Select(user => new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Email = user.Email,
                Avatar = user.Avatar,
            });

        if (top.HasValue && top > 0)
        {
            query = query.Take(top.Value);
        }

        return await query.ToListAsync(ct);
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
