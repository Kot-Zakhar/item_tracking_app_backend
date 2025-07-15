using ItTrAp.IdentityService.Application.Interfaces.Services;
using ItTrAp.IdentityService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ItTrAp.IdentityService.Infrastructure.Persistence;

public static class AppDbInitializer
{
    public static IServiceProvider InitializeAppDb(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();
        logger.LogInformation("Initializing application database...");

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.Database.Migrate();

        var config = scope.ServiceProvider.GetRequiredService<IOptions<GlobalConfig>>().Value;
        if (!string.IsNullOrEmpty(config.AdminEmail) && !string.IsNullOrEmpty(config.AdminPassword))
        {
            if (dbContext.Users.Any(u => u.Email == config.AdminEmail))
            {
                logger.LogInformation("Admin user already exists.");
            }
            else
            {
                var adminUser = new User { Email = config.AdminEmail };
                var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasherService>();
                var (passwordHash, salt) = passwordHasher.HashPassword(config.AdminPassword);
                adminUser.SetAuthenticationData(passwordHash, salt);

                dbContext.Users.Add(adminUser);

                var adminRole = dbContext.Roles.First(r => r.Name == Role.AdminRoleName);

                adminUser.Roles.Add(adminRole);
                adminRole.Users.Add(adminUser);

                dbContext.SaveChanges();

                logger.LogInformation("Admin user created successfully.");
            }
        }

        return provider;
    }
}