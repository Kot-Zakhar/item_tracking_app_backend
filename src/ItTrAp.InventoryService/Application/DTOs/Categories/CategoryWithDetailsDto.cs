namespace ItTrAp.InventoryService.Application.DTOs.Categories;

public class CategoryWithDetailsDto : CategoryTreeNodeDto<CategoryWithDetailsDto>
{
    public uint ItemsAmount { get; set; }
}