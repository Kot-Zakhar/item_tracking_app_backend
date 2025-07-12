using ItTrAp.IdentityService.Models;

namespace ItTrAp.IdentityService.Interfaces.Services;

public interface IJwtTokenService
{
    string GenerateAccessToken(User user, UserSession session);
}