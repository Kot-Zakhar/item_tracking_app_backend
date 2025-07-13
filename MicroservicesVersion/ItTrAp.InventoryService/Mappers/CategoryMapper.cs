using ItTrAp.InventoryService.DTOs.Categories;
using ItTrAp.InventoryService.Models;

namespace ItTrAp.InventoryService.Mappers;

public static class CategoryMapper
{
    public static CategoryDto ToDto(this Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Icon = category.Icon,
        };
    }
}
