using Application.Common.DTOs;
using Application.Users.DTOs;
using Application.Users.Interfaces;
using Application.UserSelfManagement.Interfaces;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EFPersistence.ReadRepositories;

public class EfUserReadRepository : IUserReadRepository, IUserSelfManagementReadRepository, IUserUniquenessChecker
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

    public async Task<List<UserWithDetailsDto>> GetAllFiltered(string? search, int? top, CancellationToken ct = default)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Where(user => string.IsNullOrEmpty(search) || user.FirstName.Contains(search) || user.LastName.Contains(search))
            .Select(user => new UserWithDetailsDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Email = user.Email,
                Avatar = user.Avatar,
                ItemsAmount = (uint)user.MovableInstances.Count() // Assuming Items is a navigation property
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
