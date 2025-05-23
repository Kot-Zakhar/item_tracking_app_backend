using Application.Locations.Dtos;

namespace Application.Locations.Interfaces;

public interface ILocationService
{
    Task<uint> CreateLocationAsync(CreateLocationDto dto, CancellationToken ct = default);
    Task UpdateLocationAsync(uint id, UpdateLocationDto dto, CancellationToken ct = default);
    Task DeleteLocationAsync(uint id, CancellationToken ct = default);
}