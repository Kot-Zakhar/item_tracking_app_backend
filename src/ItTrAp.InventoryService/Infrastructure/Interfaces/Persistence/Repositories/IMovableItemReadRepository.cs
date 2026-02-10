using ItTrAp.InventoryService.Application.DTOs.MovableItems;

namespace ItTrAp.InventoryService.Infrastructure.Interfaces.Persistence.Repositories;

public interface IMovableItemReadRepository
{
    Task<List<MovableItemDto>> GetAllFilteredAsync(
        MovableItemFiltersDto filters,
        CancellationToken ct = default
    );
    Task<List<MovableItemDto>> GetAllAsync(CancellationToken ct = default);
}
