using ItTrAp.ManagementService.Application.DTOs.MovableInstances;

namespace ItTrAp.ManagementService.Application.Interfaces.Repositories;

public interface IMovableInstanceReadRepository
{
    Task<List<MovableInstanceDto>> GetAllFilteredAsync(Guid itemId, MovableInstanceFiltersDto filters, CancellationToken ct = default);
    Task<MovableInstanceDto?> GetByIdAsync(Guid itemId, uint id, CancellationToken ct = default);
}