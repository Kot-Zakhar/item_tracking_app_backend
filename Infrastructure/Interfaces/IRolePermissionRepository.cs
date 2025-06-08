namespace Infrastructure.Interfaces;

public interface IRolePermissionRepository
{
    Task<bool> IsUserAdminAsync(uint userId, string v);
    Task<bool> UserHavePermissionAsync(uint userId, string permissionName);
}