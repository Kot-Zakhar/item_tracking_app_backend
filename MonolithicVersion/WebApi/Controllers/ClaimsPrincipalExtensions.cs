namespace WebApi.Controllers;

public static class ClaimsPrincipalExtensions
{
    public static uint GetId(this System.Security.Claims.ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !uint.TryParse(userIdClaim.Value, out var userId))
        {
            throw new InvalidOperationException("User ID is not available or invalid.");
        }
        return userId;
    }
}