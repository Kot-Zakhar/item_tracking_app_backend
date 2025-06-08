using Abstractions;
using Abstractions.Users;
using Infrastructure.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.EFPersistence;

public static class AppDbInitializer
{
    public static IServiceProvider InitializeAppDb(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();
        logger.LogInformation("Initializing application database...");

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        CreateDefaultRolesIfNecessary(provider, dbContext);
        EnsurePredefinedPermissions(provider, dbContext);
        CreateAdminUserIfNecessary(provider, dbContext);

        dbContext.SaveChanges();

        return provider;
    }

    private static void CreateDefaultRolesIfNecessary(this IServiceProvider provider, AppDbContext dbContext)
    {
        if (dbContext.Roles.Any())
            return;

        Enum.GetValues<PredefinedRoles>()
            .ToList()
            .ForEach(role =>
            {
                dbContext.Roles.Add(new Domain.Models.Role
                {
                    Id = (uint)role,
                    Name = Enum.GetName(role)!.ToLower()
                });
            });

        dbContext.SaveChanges();
    }

    private static void EnsurePredefinedPermissions(this IServiceProvider provider, AppDbContext dbContext)
    {
        foreach (var (role, permissions) in PredefinedPermissions.RolePermissions)
        {
            var roleEntity = dbContext.Roles.FirstOrDefault(r => r.Id == (uint)role);
            if (roleEntity == null)
            {
                throw new InvalidOperationException($"Predefined role {Enum.GetName(role)} not found.");
            }

            foreach (var permission in permissions)
            {
                var permissionEntity = dbContext.Permissions.FirstOrDefault(p => p.Name == permission);

                if (permissionEntity == null)
                {
                    permissionEntity = new Domain.Models.Permission
                    {
                        Name = permission,
                    };
                    dbContext.Permissions.Add(permissionEntity);
                }
                else
                {
                    // If the permission already exists, ensure it is not duplicated
                    if (roleEntity.Permissions.Any(p => p.Id == permissionEntity.Id))
                        continue;
                }

                roleEntity.Permissions.Add(permissionEntity);
            }

            var toUnassign = roleEntity.Permissions.Where(p => !permissions.Contains(p.Name)).ToList();
            foreach (var permissionToUnassign in toUnassign)
            {
                roleEntity.Permissions.Remove(permissionToUnassign);
            }

            dbContext.Roles.Update(roleEntity);

            dbContext.SaveChanges();
        }
    }

    private static void CreateAdminUserIfNecessary(this IServiceProvider provider, AppDbContext dbContext)
    {
        if (dbContext.Users.Any())
            return;

        var adminRole = dbContext.Roles.FirstOrDefault(r => r.Name == nameof(PredefinedRoles.Admin).ToLower());
        if (adminRole == null)
        {
            throw new InvalidOperationException("Admin role not found. Ensure roles are created before creating the admin user.");
        }

        var config = provider.GetRequiredService<IInfrastructureGlobalConfig>();
        var adminEmail = config.AdminEmail ?? "admin@example.com";
        var adminPhone = config.AdminPhone ?? "+1234567890";
        var adminPassword = config.AdminPassword ?? "admin";

        var adminUser = new Domain.Models.User
        {
            Email = adminEmail,
            Phone = adminPhone,
            FirstName = "Admin",
            LastName = "Admin",
            CreatedAt = DateTime.UtcNow,
            Roles = new List<Domain.Models.Role> { adminRole }
        };

        IPasswordHasher passwordHasher = provider.GetRequiredService<IPasswordHasher>();
        var (adminPasswordHash, adminSalt) = passwordHasher.HashPassword(adminPassword);

        adminUser.SetAuthenticationData(adminPasswordHash, adminSalt);

        dbContext.Users.Add(adminUser);

        adminRole.Users.Add(adminUser);
        dbContext.Roles.Update(adminRole);

        var logger = provider.GetRequiredService<ILogger<AppDbContext>>();
        logger.LogInformation("Admin user created:\n\temail: {Email},\n\tphone: {Phone},\n\tpassword: {Password}", adminEmail, adminPhone, adminPassword);

        dbContext.SaveChanges();
    }
}