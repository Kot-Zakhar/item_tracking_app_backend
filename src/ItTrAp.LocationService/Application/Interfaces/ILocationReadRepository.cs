using ItTrAp.LocationService.Application.DTOs;

namespace ItTrAp.LocationService.Application.Interfaces;
// TODO: Use collection interfaces instead of concrete types (IList instead of List)
public interface ILocationReadRepository
{
    Task<List<LocationDto>> GetAllFilteredAsync(LocationFiltersDto filters, CancellationToken ct = default);
    Task<LocationDto?> GetByIdAsync(uint id, CancellationToken ct = default);
    Task<IList<LocationDto>> GetLocationsByIdsAsync(IList<uint> ids, CancellationToken cancellationToken);
}