namespace ItTrAp.InventoryService.Application.DTOs.MovableItems;

public record MovableItemFiltersDto
{
    public List<Guid>? Ids { get; init; }
    public List<uint>? CategoryIds { get; init; }
    public string? Search { get; init; }
}