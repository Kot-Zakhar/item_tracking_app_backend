using Application.Common.ViewModels;
using Application.Locations.Dtos;
using Application.Locations.ViewModels;

namespace Application.Locations.Interfaces;

public interface ILocationReadRepository
{
    Task<List<LocationWithDetailsViewModel>> GetAllFilteredAsync(LocationFiltersDto filters, CancellationToken ct = default);
    Task<LocationViewModel?> GetByIdAsync(uint id, CancellationToken ct = default);
}