using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EFPersistence;

public class EFRolePermissionRepository : IRolePermissionRepository
{
    private readonly AppDbContext _dbContext;

    public EFRolePermissionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> UserHavePermissionAsync(uint userId, string adminRoleName, string permissionName)
    {
        return await _dbContext.Users
            .Include(user => user.Roles)
            .ThenInclude(role => role.Permissions)
            .AnyAsync(user => user.Id == userId && user.Roles.Any(role => role.Name == adminRoleName || role.Permissions.Any(p => p.Name == permissionName)));
    }
}