using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Users;
using Abstractions.Auth;
using Microsoft.IdentityModel.Tokens;
using Abstractions;

namespace Infrastructure.Services.Auth;

public class JwtTokenGenerator(IInfrastructureGlobalConfig config) : IJwtTokenGenerator
{
    public static readonly TimeSpan accessTokenAge = TimeSpan.FromMinutes(30);

    public string GenerateAccessToken(User user, UserSession session)
    {
        var secret = config.JwtPrivateKey;
        var domain = config.Domain;
        var key = new SymmetricSecurityKey(Encoding.Unicode.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        
        var token = new JwtSecurityToken(
            issuer: domain,
            audience: domain,
            claims:
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, domain),
                new Claim(JwtRegisteredClaimNames.Aud, session.Fingerprint.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, session.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("userAgent", session.UserAgent),
            ],
            notBefore: session.CreatedAt,
            expires: session.CreatedAt.Add(accessTokenAge),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}