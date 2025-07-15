using Microsoft.EntityFrameworkCore;

namespace ItTrAp.LocationService.Infrastructure.Persistence;

public static class AppDbInitializer
{
    public static IServiceProvider InitializeAppDb(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();
        logger.LogInformation("Initializing ItTrAp.LocationService. database...");

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.Database.Migrate();

        return provider;
    }
}