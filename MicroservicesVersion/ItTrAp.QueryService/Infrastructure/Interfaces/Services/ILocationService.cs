using ItTrAp.QueryService.Application.Filters;

namespace ItTrAp.QueryService.Infrastructure.Interfaces.Services;

public interface ILocationService
{
    Task<IList<Models.Location>> GetLocationsAsync(LocationFiltersDto filters, CancellationToken cancellationToken = default);
}