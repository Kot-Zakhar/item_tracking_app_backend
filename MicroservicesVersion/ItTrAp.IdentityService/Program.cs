using FluentValidation;
using ItTrAp.IdentityService.Interfaces.Persistence;
using ItTrAp.IdentityService.Interfaces.Persistence.Repositories;
using ItTrAp.IdentityService.Interfaces.Services;
using ItTrAp.IdentityService.Persistence;
using ItTrAp.IdentityService.Persistence.Repositories;
using ItTrAp.IdentityService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseSnakeCaseNamingConvention();
});

builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
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

var appConfig = builder.Configuration.GetSection("GlobalConfig");

builder.Services.Configure<GlobalConfig>(appConfig);

var app = builder.Build();

app.Services.InitializeAppDb();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

#if !DEBUG
    app.UseHttpsRedirection();
#endif

app.UseRouting();

app.MapControllers();

app.Run();
