using FluentValidation;
using ItTrAp.IdentityService.Infrastructure.Interfaces.Persistence;
using ItTrAp.IdentityService.Infrastructure.Interfaces.Persistence.Repositories;
using ItTrAp.IdentityService.Application.Interfaces.Services;
using ItTrAp.IdentityService.Infrastructure.Persistence;
using ItTrAp.IdentityService.Infrastructure.Persistence.Repositories;
using ItTrAp.IdentityService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var appConfig = builder.Configuration.GetSection("GlobalConfig");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseSnakeCaseNamingConvention();
});

builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        cfg.LicenseKey = appConfig["MediatrLicenseKey"];
    });

builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddRouting();

builder.Services.AddControllers();

builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<IUserSessionRepository, EFUserSessionRepository>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();

builder.Services.Configure<GlobalConfig>(appConfig);

var app = builder.Build();

app.Services.InitializeAppDb();

// #if !DEBUG
//     app.UseHttpsRedirection();
// #endif

app.UseRouting();

app.MapControllers();

app.Run();
