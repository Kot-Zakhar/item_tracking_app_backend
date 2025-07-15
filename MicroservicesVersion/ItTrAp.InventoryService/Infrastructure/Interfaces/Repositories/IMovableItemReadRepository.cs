using ItTrAp.InventoryService.Application.DTOs.MovableItems;

namespace ItTrAp.InventoryService.Infrastructure.Interfaces.Repositories;

public interface IMovableItemReadRepository
{
    Task<List<MovableItemDto>> GetAllFilteredAsync(
        MovableItemFiltersDto filters,
        CancellationToken ct = default
    );
}
