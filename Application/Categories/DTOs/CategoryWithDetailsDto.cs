using Application.Common.DTOs;

namespace Application.Categories.DTOs;
// TODO: derive from CategoryDto
public class CategoryWithDetailsDto
{
    public required CategoryDto Category { get; set; }
    public List<CategoryWithDetailsDto>? Children { get; set; }
    public CategoryWithDetailsDto? Parent { get; set; }
    public uint ItemsAmount { get; set; }
}