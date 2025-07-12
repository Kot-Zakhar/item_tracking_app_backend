using ItTrAp.InventoryService.DTOs.MovableItems;

namespace ItTrAp.InventoryService.Interfaces.Repositories;

public interface IMovableItemReadRepository
{
    Task<List<MovableItemDto>> GetAllFilteredAsync(
        MovableItemFiltersDto filters,
        CancellationToken ct = default
    );

    Task<MovableItemDto?> GetByIdAsync(uint id, CancellationToken ct = default);
}
