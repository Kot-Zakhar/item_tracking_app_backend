using Domain.Users;

namespace Infrastructure.Interfaces.Auth;

public interface IJwtTokenGenerator
{
    string GenerateAccessToken(User user, UserSession session);
}