using ItTrAp.InventoryService.Enums;

namespace ItTrAp.InventoryService.DTOs.MovableItems;

public record MovableItemFiltersDto(
    List<uint>? CategoryIds,
    string? Search
);