using Microsoft.AspNetCore.Authorization;

namespace WebApi.Auth;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public static readonly string PolicyPrefix = "has-perm";
    public HasPermissionAttribute(string permissionName)
        => Policy = $"{PolicyPrefix}:{permissionName}";
}