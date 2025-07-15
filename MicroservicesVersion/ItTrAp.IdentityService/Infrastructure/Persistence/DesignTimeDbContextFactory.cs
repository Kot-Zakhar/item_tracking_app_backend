using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ItTrAp.IdentityService.Infrastructure.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        if (args.Length == 0)
        {
            throw new ArgumentException("Connection string is required. Try 'dotnet ef database update -- \"{your_connection_string}\"'.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(args[0]);
        optionsBuilder.UseSnakeCaseNamingConvention();

        return new AppDbContext(optionsBuilder.Options);
    }
}