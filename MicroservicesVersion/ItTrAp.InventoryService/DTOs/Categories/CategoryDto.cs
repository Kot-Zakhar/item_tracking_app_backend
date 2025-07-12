namespace ItTrAp.InventoryService.DTOs.Categories;

public class CategoryDto
{
    public uint Id { get; set; }
    public required string Name { get; set; } // TODO: It was 'Title' on the FE
    public string? Icon { get; set; }
}
