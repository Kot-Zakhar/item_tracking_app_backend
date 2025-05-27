using FluentValidation;

using Application.Auth.Interfaces;
using Application.Categories.Interfaces;
using Application.Users.Interfaces;
using Application.Locations.Interfaces;
using Application.MovableItems.Interfaces;
using Domain.Users.Interfaces;
using Domain.Locations.Interfaces;
using Domain.Categories.Interfaces;
using Domain.MovableItems.Interfaces;
using Infrastructure.Persistence.Users;
using Infrastructure.Persistence;
using Infrastructure.Persistence.UserSessions;
using Infrastructure.Persistence.Categories;
using Infrastructure.Persistence.Locations;
using Infrastructure.Persistence.MovableItems;
using Infrastructure.Interfaces.Common;
using Infrastructure.Interfaces.Auth;
using Infrastructure.Interfaces.Users;
using Infrastructure.Interfaces.Categories;
using Infrastructure.Interfaces.Locations;
using Infrastructure.Interfaces.MovableItems;
using Infrastructure.Services.Users;
using Infrastructure.Services.Auth;
using Infrastructure.Services.Categories;
using Infrastructure.Services.Locations;
using Infrastructure.Services.MovableItems;
using Infrastructure.Services.MovableInstances;
using Application.MovableInstances.Interfaces;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Interfaces.MovableInstances;
using Infrastructure.Persistence.MovableInstances;

public static class DependencyInjectionContainer
{
    internal class LazyResolver<T>(IServiceProvider serviceProvider) : Lazy<T>(serviceProvider.GetRequiredService<T>) where T : class;

    public static IServiceCollection AddDependencyInversionContainer(this IServiceCollection services)
    {
        services.AddTransient(typeof(Lazy<>), typeof(LazyResolver<>));

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

        services.AddScoped<ICategoryService, CategoryService>();

        services.AddScoped<ILocationService, LocationService>();

        services.AddScoped<IMovableItemService, MovableItemService>();
        services.AddScoped<IMovableInstanceService, MovableInstanceService>();

        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IUserReadRepository, EfUserReadRepository>();
        services.AddScoped<IUserRepository, EfUserRepository>();
        services.AddScoped<IUserSessionRepository, EFUserSessionRepository>();

        services.AddScoped<ICategoryUniquenessChecker, EFCategoryReadRepository>();
        services.AddScoped<ICategoryReadRepository, EFCategoryReadRepository>();
        services.AddScoped<ICategoryRepository, EFCategoryRepository>();

        services.AddScoped<ILocationUniquenessChecker, EFLocationReadRepository>();
        services.AddScoped<ILocationReadRepository, EFLocationReadRepository>();
        services.AddScoped<ILocationRepository, EFLocationRepository>();

        services.AddScoped<IMovableItemUniquenessChecker, EfMovableItemReadRepository>();
        services.AddScoped<IMovableItemReadRepository, EfMovableItemReadRepository>();
        services.AddScoped<IMovableItemRepository, EFMovableItemRepository>();

        services.AddScoped<IMovableInstanceReadRepository, EFMovableInstanceReadRepository>();
        services.AddScoped<IMovableInstanceRepository, EFMovableInstanceRepository>();

        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        return services;
    }
}