using Domain.Users;

namespace Abstractions.Auth;

public interface IJwtTokenGenerator
{
    string GenerateAccessToken(User user, UserSession session);
}