using FluentValidation;

using Application.Auth.Interfaces;
using Application.Categories.Interfaces;
using Application.Users.Interfaces;
using Application.Locations.Interfaces;
using Application.MovableItems.Interfaces;
using Domain.Users.Interfaces;
using Domain.Locations.Interfaces;
using Domain.MovableItems.Interfaces;
using Infrastructure.EFPersistence.Users;
using Infrastructure.EFPersistence;
using Infrastructure.EFPersistence.UserSessions;
using Infrastructure.EFPersistence.Categories;
using Infrastructure.EFPersistence.Locations;
using Infrastructure.EFPersistence.MovableItems;
using Abstractions;
using Abstractions.Auth;
using Abstractions.Users;
using Infrastructure.Services.Users;
using Infrastructure.Services.Auth;
using Infrastructure.Services;
using Application.MovableInstances.Interfaces;
using Infrastructure.EFPersistence.Repositories;
using Infrastructure.Interfaces;
using Application.Reservations.Interfaces;
using Infrastructure.EFPersistence.Reservations;
using Infrastructure.EFPersistence.MovableInstances;

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
        services.AddScoped<IReservationService, ReservationService>();

        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));

        services.AddScoped<IUserReadRepository, EfUserReadRepository>();
        services.AddScoped<IUserRepository, EfUserRepository>();
        services.AddScoped<IUserSessionRepository, EFUserSessionRepository>();

        services.AddScoped<ICategoryUniquenessChecker, EFCategoryReadRepository>();
        services.AddScoped<ICategoryReadRepository, EFCategoryReadRepository>();

        services.AddScoped<ILocationUniquenessChecker, EFLocationReadRepository>();
        services.AddScoped<ILocationReadRepository, EFLocationReadRepository>();
        services.AddScoped<ILocationRepository, EFLocationRepository>();

        services.AddScoped<IMovableItemUniquenessChecker, EfMovableItemReadRepository>();
        services.AddScoped<IMovableItemReadRepository, EfMovableItemReadRepository>();

        services.AddScoped<IMovableInstanceRepository, EFMovableInstanceRepository>();
        services.AddScoped<IMovableInstanceReadRepository, EFMovableInstanceReadRepository>();

        services.AddScoped<IReservationReadRepository, EFReservationsReadRepository>();

        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        return services;
    }
}