namespace Abstractions.Auth;

public interface IAuthorizationService
{
    Task<bool> UserHavePermissionAsync(uint userId, string permissionName);
}