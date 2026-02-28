using Bogus;
using ItTrAp.UserService.Application.Commands;
using ItTrAp.UserService.Application.DTOs;
using ItTrAp.UserService.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ItTrAp.UserService.Jobs;

public class DataSeedingJob(
    ILogger<DataSeedingJob> logger,
    IServiceProvider serviceProvider,
    IOptions<GlobalConfig> config) : BackgroundService
{
    private const int UserCount = 20;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!config.Value.SeedOnStartup)
        {
            logger.LogInformation("Data seeding is disabled (SeedOnStartup = false). Skipping.");
            return;
        }

        // Wait for infrastructure (localstack, DB migrations, admin user creation) to be ready
        await Task.Delay(TimeSpan.FromSeconds(70), stoppingToken);

        using var checkScope = serviceProvider.CreateScope();
        var dbContext = checkScope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Skip if there are already users beyond the admin user
        var userCount = await dbContext.Users.CountAsync(stoppingToken);
        if (userCount > 1)
        {
            logger.LogInformation("Database already contains {Count} users. Skipping seed.", userCount);
            return;
        }

        logger.LogInformation("Starting user data seeding with {Count} fake users...", UserCount);

        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var faker = new Faker<CreateUserDto>()
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName).ToLowerInvariant())
            .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber("+1##########"));

        var users = faker.Generate(UserCount);
        var seeded = 0;

        foreach (var user in users)
        {
            try
            {
                await mediator.Send(new CreateUserCommand(user), stoppingToken);
                seeded++;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to seed user {Email} (may already exist).", user.Email);
            }
        }

        logger.LogInformation("User data seeding completed. Seeded {Seeded}/{Total} users.", seeded, UserCount);
    }
}
