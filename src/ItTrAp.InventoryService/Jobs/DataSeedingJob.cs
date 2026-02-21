using Bogus;
using ItTrAp.InventoryService.Application.Commands.Categories;
using ItTrAp.InventoryService.Application.Commands.MovableInstances;
using ItTrAp.InventoryService.Application.DTOs.Categories;
using ItTrAp.InventoryService.Application.DTOs.MovableItems;
using ItTrAp.InventoryService.Infrastructure.Persistence;
using ItTrAp.Commands.MovableItems;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ItTrAp.InventoryService.Jobs;

public class DataSeedingJob(
    ILogger<DataSeedingJob> logger,
    IServiceProvider serviceProvider,
    IOptions<GlobalConfig> config) : BackgroundService
{
    private const int ParentCategoryCount = 5;
    private const int MaxChildrenPerCategory = 4;
    private const int ItemsPerCategory = 3;
    private const int InstancesPerItem = 2;

    private static readonly string[] Icons =
    [
        "inventory_2", "electric_bolt", "chair", "attach_file", "build", "health_and_safety",
        "laptop_mac", "monitor", "mouse", "file_cabinet", "weekend", "description",
        "edit", "hardware", "power", "straighten", "home_repair_service", "backpack",
        "label", "square_foot", "devices", "kitchen", "local_shipping", "precision_manufacturing",
        "memory", "cable", "router", "print", "headphones", "videocam"
    ];

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!config.Value.SeedOnStartup)
        {
            logger.LogInformation("Data seeding is disabled (SeedOnStartup = false). Skipping.");
            return;
        }

        // Wait for infrastructure (DB migrations, Mongo) to be ready
        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

        using var checkScope = serviceProvider.CreateScope();
        var dbContext = checkScope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (await dbContext.Categories.AnyAsync(stoppingToken))
        {
            logger.LogInformation("Database already contains categories. Skipping seed.");
            return;
        }

        logger.LogInformation("Starting inventory data seeding...");

        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        // Step 1: Seed categories (parent + children)
        var categoryIds = await SeedCategoriesAsync(mediator, stoppingToken);

        // Step 2: Seed movable items for all categories
        var itemIds = await SeedMovableItemsAsync(mediator, categoryIds, stoppingToken);

        // Step 3: Seed instances for each item
        await SeedMovableInstancesAsync(mediator, itemIds, stoppingToken);

        logger.LogInformation("Inventory data seeding completed.");
    }

    private async Task<List<uint>> SeedCategoriesAsync(IMediator mediator, CancellationToken ct)
    {
        var allCategoryIds = new List<uint>();
        var faker = new Faker();
        var usedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        for (var i = 0; i < ParentCategoryCount; i++)
        {
            var parentName = GenerateUniqueName(() => faker.Commerce.Department(), usedNames);
            var parentIcon = faker.PickRandom(Icons);

            uint parentId;
            try
            {
                parentId = await mediator.Send(new CreateCategoryCommand(new CreateCategoryDto
                {
                    Name = parentName,
                    Icon = parentIcon,
                    ParentId = null
                }), ct);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to seed parent category {Name} (may already exist). Skipping subtree.", parentName);
                continue;
            }

            allCategoryIds.Add(parentId);

            var childCount = faker.Random.Int(0, MaxChildrenPerCategory);

            for (var j = 0; j < childCount; j++)
            {
                var childName = GenerateUniqueName(() => faker.Commerce.ProductAdjective() + " " + faker.Commerce.ProductMaterial(), usedNames);
                var childIcon = faker.PickRandom(Icons);

                try
                {
                    var childId = await mediator.Send(new CreateCategoryCommand(new CreateCategoryDto
                    {
                        Name = childName,
                        Icon = childIcon,
                        ParentId = parentId
                    }), ct);
                    allCategoryIds.Add(childId);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Failed to seed child category {Name} (may already exist).", childName);
                }
            }
        }

        logger.LogInformation("Seeded {Count} categories.", allCategoryIds.Count);
        return allCategoryIds;
    }

    private static string GenerateUniqueName(Func<string> generator, HashSet<string> usedNames)
    {
        string name;
        var attempts = 0;
        do
        {
            name = generator();
            attempts++;
        } while (!usedNames.Add(name) && attempts < 50);

        return name;
    }

    private async Task<List<Guid>> SeedMovableItemsAsync(IMediator mediator, List<uint> categoryIds, CancellationToken ct)
    {
        var itemIds = new List<Guid>();
        var faker = new Faker();

        foreach (var categoryId in categoryIds)
        {
            for (var i = 0; i < ItemsPerCategory; i++)
            {
                var item = new CreateMovableItemDto
                {
                    Name = faker.Commerce.ProductName(),
                    Description = faker.Commerce.ProductDescription(),
                    CategoryId = categoryId,
                    ImgSrc = faker.Image.PicsumUrl(),
                    ExtraData = null
                };

                try
                {
                    var itemId = await mediator.Send(new CreateMovableItemCommand(item), ct);
                    itemIds.Add(itemId);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Failed to seed item {Name} (may already exist).", item.Name);
                }
            }
        }

        logger.LogInformation("Seeded {Count} movable items.", itemIds.Count);
        return itemIds;
    }

    private async Task SeedMovableInstancesAsync(IMediator mediator, List<Guid> itemIds, CancellationToken ct)
    {
        var seeded = 0;

        foreach (var itemId in itemIds)
        {
            for (var i = 0; i < InstancesPerItem; i++)
            {
                try
                {
                    await mediator.Send(new CreateMovableInstanceCommand(itemId), ct);
                    seeded++;
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Failed to seed instance for item {ItemId}.", itemId);
                }
            }
        }

        logger.LogInformation("Seeded {Count} movable instances.", seeded);
    }
}
