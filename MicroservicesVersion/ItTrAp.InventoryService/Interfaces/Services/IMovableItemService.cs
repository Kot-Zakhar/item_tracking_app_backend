using ItTrAp.InventoryService.DTOs.MovableItems;

namespace ItTrAp.InventoryService.Interfaces.Services;

public interface IMovableItemService
{
    Task<MovableItemWithCategoryDto?> GetByIdAsync(Guid itemId, CancellationToken ct = default);
    Task<Guid> CreateAsync(CreateMovableItemDto movableItem, CancellationToken ct = default);
    Task UpdateAsync(Guid id, UpdateMovableItemDto movableItem, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
