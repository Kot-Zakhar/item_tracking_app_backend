using Application.Common.ViewModels;
using Application.MovableInstances.Dtos;

namespace Application.MovableInstances.Interfaces;

public interface IMovableInstanceReadRepository
{
    Task<List<MovableInstanceViewModel>> GetAllFilteredAsync(uint itemId, MovableInstanceFiltersDto filters, CancellationToken ct = default);
    Task<MovableInstanceViewModel?> GetByIdAsync(uint itemId, uint id, CancellationToken ct = default);
}