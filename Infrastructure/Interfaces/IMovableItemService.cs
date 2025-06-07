using Domain.Models;

namespace Infrastructure.Interfaces;

public interface IMovableItemService
{
    Task<MovableItem?> GetByIdAsync(uint itemId, CancellationToken ct = default);
}