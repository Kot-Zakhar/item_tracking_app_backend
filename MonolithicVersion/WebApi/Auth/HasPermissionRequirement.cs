using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Auth;

public class HasPermissionRequirement : IAuthorizationRequirement
{
    public string PermissionName { get; }

    public HasPermissionRequirement(string permissionName)
    {
        PermissionName = permissionName ?? throw new ArgumentNullException(nameof(permissionName));
    }
}

public class HasPermissionHandler : AuthorizationHandler<HasPermissionRequirement>
{
    private readonly Abstractions.Auth.IAuthorizationService _service;
    public HasPermissionHandler(Abstractions.Auth.IAuthorizationService authorizationService)
    {
        _service = authorizationService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return;

        if (!uint.TryParse(userId, out var parsedUserId))
            return;

        if (await _service.UserHavePermissionAsync(parsedUserId, requirement.PermissionName))
            context.Succeed(requirement);
    }
}