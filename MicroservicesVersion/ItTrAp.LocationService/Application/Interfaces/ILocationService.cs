using ItTrAp.LocationService.Application.DTOs;

namespace ItTrAp.LocationService.Application.Interfaces;

public interface ILocationService
{
    Task<uint> CreateLocationAsync(CreateLocationDto dto, CancellationToken ct = default);
    Task UpdateLocationAsync(uint id, UpdateLocationDto dto, CancellationToken ct = default);
    Task DeleteLocationAsync(uint id, CancellationToken ct = default);
}