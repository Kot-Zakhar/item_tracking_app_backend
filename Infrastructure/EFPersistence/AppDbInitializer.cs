using Microsoft.EntityFrameworkCore;
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

        dbContext.Database.EnsureCreated();
        dbContext.Database.Migrate();

        return provider;
    }
}