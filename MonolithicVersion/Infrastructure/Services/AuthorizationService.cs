using Infrastructure.Constants;
using Infrastructure.Interfaces.Persistence.Repositories;
using Infrastructure.Interfaces.Services;

namespace Infrastructure.Services;

// TODO: Add cache

public class AuthorizationService(
    IRolePermissionRepository repo
) : IAuthorizationService
{
    public async Task<bool> UserHavePermissionAsync(uint userId, string permissionName)
    {
        if (string.IsNullOrEmpty(permissionName))
        {
            throw new ArgumentNullException(nameof(permissionName));
        }
        
        return await repo.UserHavePermissionAsync(userId, SecurityConstants.Roles.Admin, permissionName);
    }
}