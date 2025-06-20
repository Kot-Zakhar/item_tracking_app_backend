using Abstractions.Auth;
using Infrastructure.Constants;
using Infrastructure.Interfaces;

namespace Infrastructure.Services.Auth;

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