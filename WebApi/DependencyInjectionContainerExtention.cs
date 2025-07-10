using Application.Auth.Interfaces;
using Application.Categories.Interfaces;
using Application.Users.Interfaces;
using Application.Locations.Interfaces;
using Application.MovableItems.Interfaces;
using Application.MovableInstances.Interfaces;
using Application.Reservations.Interfaces;
using Application.UserSelfManagement.Interfaces;
using Application.Files.Interfaces;

using Domain.Interfaces;

using Infrastructure.Services;
using Infrastructure.EFPersistence;
using Infrastructure.EFPersistence.ReadRepositories;
using Infrastructure.EFPersistence.Repositories;
using Infrastructure.Interfaces.Services;
using Infrastructure.Interfaces.Persistence;
using Infrastructure.Interfaces.Persistence.Repositories;


public static class DependencyInjectionContainerExtention
{
    internal class LazyResolver<T>(IServiceProvider serviceProvider) : Lazy<T>(serviceProvider.GetRequiredService<T>) where T : class;

    public static IServiceCollection AddDependencyInversionContainer(this IServiceCollection services)
    {
        services.AddTransient(typeof(Lazy<>), typeof(LazyResolver<>));

        services.AddSingleton<IPasswordHasherService, PasswordHasherService>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserSelfManagementService, UserService>();
        services.AddScoped<IUserUniquenessChecker, EfUserReadRepository>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<Application.MovableItems.Interfaces.IMovableItemService, MovableItemService>();
        services.AddScoped<Infrastructure.Interfaces.Services.IMovableItemService, MovableItemService>();
        services.AddScoped<IMovableInstanceService, MovableInstanceService>();
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<IQrService, QrService>();
        services.AddScoped<IFileService, FileService>();

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