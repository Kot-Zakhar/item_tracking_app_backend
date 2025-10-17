using ItTrAp.InventoryService.Application.DTOs.MovableInstances;

namespace ItTrAp.InventoryService.Application.Interfaces.Repositories;

public interface IMovableInstanceReadRepository
{
    Task<List<MovableInstanceDto>> GetAllAsync(Guid itemId, CancellationToken ct = default);
    Task<MovableInstanceDto?> GetByIdAsync(Guid itemId, uint id, CancellationToken ct = default);
    Task<IList<int>> GetInstanceAmountsByItemIdsAsync(IList<Guid> itemIds, CancellationToken ct = default);
}