using Domain.Aggregates.MovableItems;

namespace Infrastructure.Interfaces.Services;

public interface IMovableItemService
{
    Task<MovableItem?> GetByIdAsync(uint itemId, CancellationToken ct = default);
}