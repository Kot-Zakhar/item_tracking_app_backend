using Grpc.Net.Client;
using ItTrAp.QueryService.Domain.Enums;
using ItTrAp.QueryService.Infrastructure.Interfaces.Services;
using ItTrAp.QueryService.Infrastructure.Protos;
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
            var managementClient = new ManagementServer.ManagementServerClient(managementChannel);

            var request = new GetInstanceAmountRequest();
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

    public async Task<Dictionary<Guid, List<KeyValuePair<MovableInstanceStatus, uint>>>> GetUserStatusesForItemsAsync(List<Guid> itemIds, CancellationToken cancellationToken = default)
    {
        try
        {
            using var managementChannel = GrpcChannel.ForAddress(_managementServiceAddress);
            var managementClient = new ManagementServer.ManagementServerClient(managementChannel);

            var request = new GetUserStatusesForItemsRequest();
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