using Grpc.Net.Client;
using ItTrAp.QueryService.Application.Filters;
using ItTrAp.QueryService.Application.Responses;
using ItTrAp.QueryService.Application.Interfaces.Services;
using ItTrAp.QueryService.Infrastructure.Protos;
using Microsoft.Extensions.Options;

namespace ItTrAp.QueryService.Infrastructure.Services;

public class LocationGrpcService(ILogger<LocationGrpcService> logger, IOptions<GlobalConfig> config) : ILocationService
{
    private readonly string _locationServiceAddress = config.Value.LocationServiceAddress;

    public async Task<IList<LocationViewModel>> GetLocationsAsync(LocationFiltersDto? filters, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching locations from LocationService at {LocationServiceAddress}", _locationServiceAddress);

        try
        {
            List<Location> locations;

            using (var locationChannel = GrpcChannel.ForAddress(_locationServiceAddress))
            {
                var locationClient = new LocationServer.LocationServerClient(locationChannel);

                var request = new GetLocationsRequest();

                request.Filter = new LocationFilter
                {
                    Search = filters?.Search ?? string.Empty,
                    Floor = filters?.Floor,
                };

                var response = await locationClient.GetLocationsAsync(request, cancellationToken: cancellationToken);
                locations = response.Locations.ToList();
            }

            return locations
                .Select(loc => new LocationViewModel
                {
                    Id = loc.Id,
                    Name = loc.Name,
                    Floor = (sbyte)loc.Floor,
                    Department = loc.Department,
                    CreatedAt = new DateTime((long)loc.CreatedAt)
                })
                .ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while fetching locations");
            throw;
        }
    }

    public async Task<IList<LocationViewModel>> GetLocationsByIdsAsync(List<uint> locationsIds, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching locations by IDs from LocationService at {LocationServiceAddress}", _locationServiceAddress);

        try
        {
            List<Location> locations;

            using (var locationChannel = GrpcChannel.ForAddress(_locationServiceAddress))
            {
                var locationClient = new LocationServer.LocationServerClient(locationChannel);

                var request = new GetLocationsByIdsRequest();
                request.Ids.AddRange(locationsIds);

                var response = await locationClient.GetLocationsByIdsAsync(request, cancellationToken: cancellationToken);
                locations = response.Locations.ToList();
            }

            return locations
                .Select(loc => new LocationViewModel
                {
                    Id = loc.Id,
                    Name = loc.Name,
                    Floor = (sbyte)loc.Floor,
                    Department = loc.Department,
                    CreatedAt = new DateTime((long)loc.CreatedAt)
                })
                .ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while fetching locations by IDs");
            throw;
        }
    }
}