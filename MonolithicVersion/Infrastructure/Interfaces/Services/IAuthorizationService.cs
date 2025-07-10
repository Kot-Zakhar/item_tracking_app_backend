namespace Infrastructure.Interfaces.Services;

public interface IAuthorizationService
{
    Task<bool> UserHavePermissionAsync(uint userId, string permissionName);
}