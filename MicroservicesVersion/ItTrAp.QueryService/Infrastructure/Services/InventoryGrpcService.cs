using ItTrAp.QueryService.Application.Interfaces.Services;
using ItTrAp.QueryService.Application.Responses;
using Microsoft.Extensions.Options;
using ItTrAp.QueryService.Application.DTOs;

namespace ItTrAp.QueryService.Infrastructure.Services;

public class InventoryGrpcService(ILogger<InventoryGrpcService> logger, IOptions<GlobalConfig> config) : IInventoryService
{
    private readonly string _inventoryServiceAddress = config.Value.InventoryServiceAddress;

    public async Task<IList<MovableItemViewModel>> GetMovableItemsAsync(List<Guid>? itemIds, List<uint>? categoryIds, string? search, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching movable items from InventoryService at {InventoryServiceAddress}", _inventoryServiceAddress);

        try
        {
            using var inventoryChannel = Grpc.Net.Client.GrpcChannel.ForAddress(_inventoryServiceAddress);
            var inventoryClient = new Protos.InventoryServer.InventoryServerClient(inventoryChannel);

            var request = new Protos.GetMovableItemsRequest
            {
                Search = search,
            };

            if (categoryIds != null)
            {
                request.CategoryIds.AddRange(categoryIds);
            }

            if (itemIds != null)
            {
                request.ItemIds.AddRange(itemIds.Select(id => id.ToString()));
            }

            var response = await inventoryClient.GetMovableItemsAsync(request, cancellationToken: cancellationToken);
            return response.Items.Select(item => new MovableItemViewModel
            {
                Id = Guid.Parse(item.Id),
                Name = item.Name,
                Description = item.Description,
                Category = new CategoryViewModel
                {
                    Id = item.Category.Id,
                    Name = item.Category.Name,
                    Icon = item.Category.Icon,
                },
                CreatedAt = item.CreatedAt.ToDateTime(),
                ImgSrc = item.ImgSrc,

            }).ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while fetching movable items");
            throw;
        }
    }

    public async Task<IList<uint>> GetInstanceAmountsByItemIdsAsync(List<Guid> itemIds, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching movable item instance amounts from InventoryService at {InventoryServiceAddress}", _inventoryServiceAddress);

        try
        {
            using var inventoryChannel = Grpc.Net.Client.GrpcChannel.ForAddress(_inventoryServiceAddress);
            var inventoryClient = new Protos.InventoryServer.InventoryServerClient(inventoryChannel);

            var request = new Protos.GetInstanceAmountsByItemIdsRequest();
            request.Ids.AddRange(itemIds.Select(id => id.ToString()));

            var response = await inventoryClient.GetInstanceAmountsByItemIdsAsync(request, cancellationToken: cancellationToken);
            return response.Amounts.ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while fetching movable item instance amounts");
            throw;
        }
    }

    public async Task<IList<MovableInstanceDto>> GetMovableInstancesByItemIdAsync(Guid movableItemId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching movable instances for item {MovableItemId} from InventoryService at {InventoryServiceAddress}", movableItemId, _inventoryServiceAddress);

        try
        {
            using var inventoryChannel = Grpc.Net.Client.GrpcChannel.ForAddress(_inventoryServiceAddress);
            var inventoryClient = new Protos.InventoryServer.InventoryServerClient(inventoryChannel);

            var request = new Protos.GetMovableInstancesByItemIdRequest
            {
                ItemId = movableItemId.ToString()
            };

            var response = await inventoryClient.GetMovableInstancesByItemIdAsync(request, cancellationToken: cancellationToken);
            return response.Instances.Select(instance => new MovableInstanceDto
            {
                Id = instance.Id,
                MovableItemId = Guid.Parse(instance.MovableItemId),
                CreatedAt = instance.CreatedAt.ToDateTime()
            }).ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while fetching movable instances");
            throw;
        }
    }
}