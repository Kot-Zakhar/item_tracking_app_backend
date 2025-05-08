using Microsoft.EntityFrameworkCore;

namespace Database;

public static class NpgsqlDbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder UseNpgsqlWithSnakeCase(this DbContextOptionsBuilder optionsBuilder, string? connectionString)
    {
        optionsBuilder.UseNpgsql(connectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        return optionsBuilder;
    }
}