namespace ItTrAp.InventoryService.Application.DTOs.MovableItems;

public record MovableItemFiltersDto(
    List<uint>? CategoryIds,
    string? Search
);