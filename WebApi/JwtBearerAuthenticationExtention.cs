using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace WebApi;

public static class JwtBearerAuthenticationExtension
{
    public static AuthenticationBuilder AddConfiguredJwtBearerAuthentication(this AuthenticationBuilder builder,
        IConfiguration configuration)
    {
        return builder
            .AddJwtBearer(options =>
            {
                var key = configuration["JwtPrivateKey"];
                if (string.IsNullOrEmpty(key))
                {
                    throw new InvalidOperationException("JWT private key is not configured.");
                }

                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Unicode.GetBytes(key));

                options.Authority = configuration["Domain"];
                options.Audience = configuration["Domain"];
#if DEBUG
                options.RequireHttpsMetadata = false;
#endif
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Domain"],
                    ValidAudience = configuration["Domain"],
                    IssuerSigningKey = securityKey,
                    ValidateLifetime = true,
                };

            });
    }
}