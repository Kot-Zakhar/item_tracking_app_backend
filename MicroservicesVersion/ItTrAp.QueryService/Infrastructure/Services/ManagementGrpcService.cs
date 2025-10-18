using Grpc.Net.Client;
using ItTrAp.QueryService.Domain.Enums;
using ItTrAp.QueryService.Infrastructure.DTOs;
using ItTrAp.QueryService.Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace ItTrAp.QueryService.Infrastructure.Services;

public class ManagementGrpcService(ILogger<ManagementGrpcService> logger, IOptions<GlobalConfig> config) : IManagementService
{
    private readonly string _managementServiceAddress = config.Value.ManagementServiceAddress;

    public async Task<List<uint>> GetInstanceAmountsInLocationsAsync(List<uint> locationIds, CancellationToken cancellationToken = default)
    {
        try
        {
            using var managementChannel = GrpcChannel.ForAddress(_managementServiceAddress);
            var managementClient = new Protos.ManagementServer.ManagementServerClient(managementChannel);

            var request = new Protos.GetInstanceAmountRequest();
            request.Ids.AddRange(locationIds);

            var response = await managementClient.GetInstanceAmountInLocationsAsync(request, cancellationToken: cancellationToken);
            return response.Amounts.ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching instance amounts from Management service");
            throw;
        }
    }

    public async Task<IList<MovableInstanceStatusDto>> GetInstanceStatusesByItemAsync(Guid movableItemId, CancellationToken cancellationToken)
    {
        try
        {
            using var managementChannel = GrpcChannel.ForAddress(_managementServiceAddress);
            var managementClient = new Protos.ManagementServer.ManagementServerClient(managementChannel);

            var request = new Protos.GetInstanceStatusesByItemRequest
            {
                ItemId = movableItemId.ToString()
            };

            var response = await managementClient.GetInstanceStatusesByItemAsync(request, cancellationToken: cancellationToken);

            var result = response.Statuses.Select(s => new MovableInstanceStatusDto
            {
                Id = s.Id,
                ItemId = Guid.Parse(s.ItemId),
                Status = (MovableInstanceStatus)s.Status,
                LocationId = s.LocationId == 0 ? null : s.LocationId,
                UserId = s.UserId == 0 ? null : s.UserId
            }).ToList();

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching instance statuses from Management service");
            throw;
        }
    }

    public async Task<Dictionary<Guid, List<KeyValuePair<MovableInstanceStatus, uint>>>> GetUserStatusesForItemsAsync(List<Guid> itemIds, CancellationToken cancellationToken = default)
    {
        try
        {
            using var managementChannel = GrpcChannel.ForAddress(_managementServiceAddress);
            var managementClient = new Protos.ManagementServer.ManagementServerClient(managementChannel);

            var request = new Protos.GetUserStatusesForItemsRequest();
            request.Ids.AddRange(itemIds.Select(id => id.ToString()));

            var response = await managementClient.GetUserStatusesForItemsAsync(request, cancellationToken: cancellationToken);

            var result = new Dictionary<Guid, List<KeyValuePair<MovableInstanceStatus, uint>>>();

            foreach (var itemStatus in response.UserStatusesForItems)
            {
                var itemId = Guid.Parse(itemStatus.ItemId);
                var statusList = itemStatus.UserStatuses
                    .Select(sc => new KeyValuePair<MovableInstanceStatus, uint>((MovableInstanceStatus)sc.Status, sc.UserId))
                    .ToList();

                result[itemId] = statusList;
            }

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching user statuses from Management service");
            throw;
        }
    }

}