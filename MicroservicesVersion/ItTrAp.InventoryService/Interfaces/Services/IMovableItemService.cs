using ItTrAp.InventoryService.DTOs.MovableItems;
using ItTrAp.InventoryService.Models;

namespace ItTrAp.InventoryService.Interfaces.Services;

public interface IMovableItemService
{
    Task<MovableItem?> GetByIdAsync(uint itemId, CancellationToken ct = default);
    Task<uint> CreateAsync(CreateMovableItemDto movableItem, CancellationToken ct = default);
    Task UpdateAsync(uint id, UpdateMovableItemDto movableItem, CancellationToken ct = default);
    Task DeleteAsync(uint id, CancellationToken ct = default);
}
