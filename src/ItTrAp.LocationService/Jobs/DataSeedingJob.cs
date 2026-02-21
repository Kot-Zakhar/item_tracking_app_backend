using Bogus;
using ItTrAp.LocationService.Application.Commands;
using ItTrAp.LocationService.Application.DTOs;
using ItTrAp.LocationService.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ItTrAp.LocationService.Jobs;

public class DataSeedingJob(
    ILogger<DataSeedingJob> logger,
    IServiceProvider serviceProvider,
    IOptions<GlobalConfig> config) : BackgroundService
{
    private const int LocationCount = 15;

    private static readonly string[] Departments = [
        "Engineering", "Marketing", "Sales", "HR", "Finance",
        "Operations", "IT", "Legal", "R&D", "Support"
    ];

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!config.Value.SeedOnStartup)
        {
            logger.LogInformation("Data seeding is disabled (SeedOnStartup = false). Skipping.");
            return;
        }

        // Wait for infrastructure (DB migrations) to be ready
        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

        using var checkScope = serviceProvider.CreateScope();
        var dbContext = checkScope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (await dbContext.Locations.AnyAsync(stoppingToken))
        {
            logger.LogInformation("Database already contains locations. Skipping seed.");
            return;
        }

        logger.LogInformation("Starting location data seeding with {Count} fake locations...", LocationCount);

        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var faker = new Faker();
        var seeded = 0;

        for (var i = 0; i < LocationCount; i++)
        {
            var location = new CreateLocationDto
            {
                Name = $"{faker.Commerce.Department()} - {faker.Address.BuildingNumber()}",
                Floor = (sbyte)faker.Random.Int(-1, 10),
                Department = faker.PickRandom(Departments)
            };

            try
            {
                await mediator.Send(new CreateLocationCommand(location), stoppingToken);
                seeded++;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to seed location {Name} (may already exist).", location.Name);
            }
        }

        logger.LogInformation("Location data seeding completed. Seeded {Seeded}/{Total} locations.", seeded, LocationCount);
    }
}
