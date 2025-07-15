using ItTrAp.InventoryService.Application.DTOs.Categories;

namespace ItTrAp.InventoryService.Application.DTOs.MovableItems;

public class MovableItemWithCategoryDto : MovableItemDto
{
    public required CategoryDto Category { get; set; }
}