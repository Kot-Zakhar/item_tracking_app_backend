using Infrastructure.Persistence.Users;
using Infrastructure.Services.Users;
using Domain.Users;
using Application.Users.Interfaces;
using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.UserSessions;
using Infrastructure.Services.Auth;
using Application.Auth.Interfaces;
using FluentValidation;
using Infrastructure.Interfaces.Common;
using Infrastructure.Interfaces.Auth;

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

        services.AddScoped<IUserService, UserService>();
        
        services.AddScoped<IUserUniquenessChecker, EfUserReadRepository>();
        
        services.AddScoped<IUserReadRepository, EfUserReadRepository>();
        services.AddScoped<IUserRepository, EfUserRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        return services;
    }
}