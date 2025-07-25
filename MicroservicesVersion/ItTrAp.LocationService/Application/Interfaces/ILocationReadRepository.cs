using ItTrAp.LocationService.Application.DTOs;

namespace ItTrAp.LocationService.Application.Interfaces;

public interface ILocationReadRepository
{
    Task<List<LocationDto>> GetAllFilteredAsync(LocationFiltersDto filters, CancellationToken ct = default);
    Task<LocationDto?> GetByIdAsync(uint id, CancellationToken ct = default);
}