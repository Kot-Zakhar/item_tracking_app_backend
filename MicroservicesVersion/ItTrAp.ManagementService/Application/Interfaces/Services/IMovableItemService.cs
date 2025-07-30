using ItTrAp.ManagementService.Domain.Aggregates;

namespace ItTrAp.ManagementService.Application.Interfaces.Services;

public interface IMovableItemService
{
    Task<MovableItem?> GetByIdAsync(Guid itemId, CancellationToken ct = default);
    Task CreateAsync(Guid movableItemId, CancellationToken cancellationToken);
    Task DeleteAsync(Guid movableItemId, CancellationToken cancellationToken);
}