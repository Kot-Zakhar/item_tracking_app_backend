using ItTrAp.QueryService.Application.Filters;
using ItTrAp.QueryService.Application.Responses;

namespace ItTrAp.QueryService.Infrastructure.Interfaces.Services;

public interface ILocationService
{
    Task<IList<LocationViewModel>> GetLocationsAsync(LocationFiltersDto? filters, CancellationToken cancellationToken = default);
    Task<IList<LocationViewModel>> GetLocationsByIdsAsync(List<uint> locationsIds, CancellationToken cancellationToken = default);
}