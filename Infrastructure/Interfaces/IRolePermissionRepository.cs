namespace Infrastructure.Interfaces;

public interface IRolePermissionRepository
{
    Task<bool> UserHavePermissionAsync(uint userId, string adminRoleName, string permissionName);
}