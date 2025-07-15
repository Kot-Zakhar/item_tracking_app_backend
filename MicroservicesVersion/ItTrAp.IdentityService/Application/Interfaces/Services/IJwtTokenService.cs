using ItTrAp.IdentityService.Domain;

namespace ItTrAp.IdentityService.Application.Interfaces.Services;

public interface IJwtTokenService
{
    string GenerateAccessToken(User user, UserSession session);
}