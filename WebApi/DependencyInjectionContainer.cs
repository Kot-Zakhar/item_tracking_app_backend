using FluentValidation;

using Application.Auth.Interfaces;
using Application.Users.Interfaces;
using Infrastructure.Persistence.Users;
using Infrastructure.Services.Users;
using Infrastructure.Persistence;
using Infrastructure.Persistence.UserSessions;
using Infrastructure.Services.Auth;
using Infrastructure.Interfaces.Common;
using Infrastructure.Interfaces.Auth;
using Infrastructure.Interfaces.Users;

public static class DependencyInjectionContainer
{
    public static IServiceCollection AddDependencyInversionContainer(this IServiceCollection services)
    {
        services.AddSingleton<IInfrastructureGlobalConfig, GlobalConfig>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserUniquenessChecker, EfUserReadRepository>();
        services.AddScoped<IAuthService, AuthService>();
        
        
        services.AddScoped<IUserReadRepository, EfUserReadRepository>();
        services.AddScoped<IUserRepository, EfUserRepository>();
        services.AddScoped<IUserSessionRepository, EFUserSessionRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        return services;
    }
}