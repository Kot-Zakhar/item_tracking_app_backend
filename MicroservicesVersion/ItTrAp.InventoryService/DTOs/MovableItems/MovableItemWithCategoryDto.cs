using ItTrAp.InventoryService.DTOs.Categories;

namespace ItTrAp.InventoryService.DTOs.MovableItems;

public class MovableItemWithCategoryDto : MovableItemDto
{
    public required CategoryDto Category { get; set; }
}