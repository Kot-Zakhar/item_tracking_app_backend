using ItTrAp.InventoryService.Application.DTOs.Categories;
using ItTrAp.InventoryService.Domain.Aggregates;

namespace ItTrAp.InventoryService.Infrastructure.Mappers;

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
