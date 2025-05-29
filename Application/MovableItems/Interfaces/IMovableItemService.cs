using Application.MovableItems.DTOs;
using Domain.MovableItems;

namespace Application.MovableItems.Interfaces;

public interface IMovableItemService
{
    Task<uint> CreateAsync(CreateMovableItemDto movableItem, CancellationToken ct = default);
    Task<MovableItem?> GetByIdAsync(uint itemId, CancellationToken ct = default);
    Task UpdateAsync(uint id, UpdateMovableItemDto movableItem, CancellationToken ct = default);
    Task DeleteAsync(uint id, CancellationToken ct = default);
}
