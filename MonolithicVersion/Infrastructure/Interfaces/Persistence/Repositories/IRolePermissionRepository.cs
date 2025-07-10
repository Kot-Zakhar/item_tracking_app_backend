namespace Infrastructure.Interfaces.Persistence.Repositories;

public interface IRolePermissionRepository
{
    Task<bool> UserHavePermissionAsync(uint userId, string adminRoleName, string permissionName);
}