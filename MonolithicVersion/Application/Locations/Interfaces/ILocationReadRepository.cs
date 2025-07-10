using Application.Common.DTOs;
using Application.Locations.DTOs;

namespace Application.Locations.Interfaces;

public interface ILocationReadRepository
{
    Task<List<LocationWithDetailsDto>> GetAllFilteredAsync(LocationFiltersDto filters, CancellationToken ct = default);
    Task<LocationDto?> GetByIdAsync(uint id, CancellationToken ct = default);
}