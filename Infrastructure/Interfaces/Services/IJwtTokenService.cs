using Domain.Aggregates.Users;
using Infrastructure.Models;

namespace Infrastructure.Interfaces.Services;

public interface IJwtTokenService
{
    string GenerateAccessToken(User user, UserSession session);
}