using Abstractions;
using Abstractions.Auth;
using Abstractions.Users;

using Application.Auth.Interfaces;
using Application.Categories.Interfaces;
using Application.Users.Interfaces;
using Application.Locations.Interfaces;
using Application.MovableItems.Interfaces;
using Application.MovableInstances.Interfaces;
using Application.Reservations.Interfaces;
using Application.UserSelfManagement.Interfaces;
using Application.Files.Interfaces;

using Domain.Users.Interfaces;
using Domain.Interfaces;

using Infrastructure.EFPersistence;
using Infrastructure.Interfaces;
using Infrastructure.Services.Users;
using Infrastructure.Services.Auth;
using Infrastructure.Services;


public static class DependencyInjectionContainerExtention
{
    internal class LazyResolver<T>(IServiceProvider serviceProvider) : Lazy<T>(serviceProvider.GetRequiredService<T>) where T : class;

    public static IServiceCollection AddDependencyInversionContainer(this IServiceCollection services)
    {
        services.AddTransient(typeof(Lazy<>), typeof(LazyResolver<>));

        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserSelfManagementService, UserService>();
        services.AddScoped<IUserUniquenessChecker, EfUserReadRepository>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<Application.MovableItems.Interfaces.IMovableItemService, MovableItemService>();
        services.AddScoped<Infrastructure.Interfaces.IMovableItemService, MovableItemService>();
        services.AddScoped<IMovableInstanceService, MovableInstanceService>();
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<IQrService, QrService>();

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();

        services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));

        services.AddScoped<IUserReadRepository, EfUserReadRepository>();
        services.AddScoped<IUserRepository, EfUserRepository>();
        services.AddScoped<IUserSessionRepository, EFUserSessionRepository>();

        services.AddScoped<IUserSelfManagementReadRepository, EfUserReadRepository>();

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

        services.AddScoped<IRolePermissionRepository, EFRolePermissionRepository>();

        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        return services;
    }
}