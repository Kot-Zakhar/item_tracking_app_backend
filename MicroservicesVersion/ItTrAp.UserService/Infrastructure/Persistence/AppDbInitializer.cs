using Microsoft.EntityFrameworkCore;

namespace ItTrAp.UserService.Infrastructure.Persistence;

public static class AppDbInitializer
{
    public static IServiceProvider InitializeAppDb(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();
        logger.LogInformation("Initializing ItTrAp.UserService database...");

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.Database.Migrate();

        return provider;
    }
}