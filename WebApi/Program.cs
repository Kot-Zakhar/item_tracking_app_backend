using Database;
using Infrastructure.EFPersistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsqlWithSnakeCase(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDependencyInversionContainer();

builder.Services.AddRouting();

// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("Manager", policy =>
//     {
//         policy.RequireAuthenticatedUser();
//         policy.RequireClaim("role", "manager");
//     });

//     options.AddPolicy("User", policy =>
//     {
//         policy.RequireAuthenticatedUser();
//         policy.RequireClaim("role", "user");
//     });
// });

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["JwtPrivateKey"];
        if (string.IsNullOrEmpty(key))
        {
            throw new InvalidOperationException("JWT private key is not configured.");
        }

        var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Unicode.GetBytes(key));

        options.Authority = builder.Configuration["Domain"];
        options.Audience = builder.Configuration["Domain"];
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Domain"],
            ValidAudience = builder.Configuration["Domain"],
            IssuerSigningKey = securityKey,
            ValidateLifetime = true,
        };
    });

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// TODO: Format exceptions

#if !DEBUG
    app.UseHttpsRedirection();
#endif

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
