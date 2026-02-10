using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddRouting();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        var appConfig = builder.Configuration.GetSection("GlobalConfig");

        var key = appConfig["JwtPrivateKey"];
        if (string.IsNullOrEmpty(key))
        {
            throw new InvalidOperationException("JWT private key is not configured.");
        }

        var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Unicode.GetBytes(key));

        opts.Authority = appConfig["Domain"];
        opts.Audience = appConfig["Domain"];

// #if DEBUG
        opts.RequireHttpsMetadata = false;
// #endif

        opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidIssuer = appConfig["Domain"],
            ValidAudience = appConfig["Domain"],
            IssuerSigningKey = securityKey,
            ValidateLifetime = true,
        };
    });

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

await app.UseOcelot();

app.Run();
