using Application.Common.DTOs;
using Application.MovableInstances.DTOs;

namespace Application.MovableInstances.Interfaces;

public interface IMovableInstanceReadRepository
{
    Task<List<MovableInstanceDto>> GetAllFilteredAsync(uint itemId, MovableInstanceFiltersDto filters, CancellationToken ct = default);
    Task<MovableInstanceDto?> GetByIdAsync(uint itemId, uint id, CancellationToken ct = default);
}