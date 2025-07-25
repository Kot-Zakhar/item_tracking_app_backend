using ItTrAp.IdentityService.Domain.Aggregates;
using ItTrAp.IdentityService.Infrastructure.Models;

namespace ItTrAp.IdentityService.Application.Interfaces.Services;

public interface IJwtTokenService
{
    string GenerateAccessToken(User user, UserSession session);
}