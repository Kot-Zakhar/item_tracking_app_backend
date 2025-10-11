using Grpc.Net.Client;
using ItTrAp.QueryService.Infrastructure.Interfaces.Service;
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
}