using ItTrAp.InventoryService.DTOs.Categories;

namespace ItTrAp.InventoryService.DTOs.MovableItems;

public class MovableItemDto
{
    public uint Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required CategoryDto Category { get; set; }
    public bool Visibility { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ImgSrc { get; set; }
}