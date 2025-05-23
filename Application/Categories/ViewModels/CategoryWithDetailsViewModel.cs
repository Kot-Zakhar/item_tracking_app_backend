using Application.Common.ViewModels;

namespace Application.Categories.Interfaces;
// TODO: derive from CategoryViewModel
public class CategoryWithDetailsViewModel
{
    public required CategoryViewModel Category { get; set; }
    public List<CategoryWithDetailsViewModel>? Children { get; set; }
    public CategoryWithDetailsViewModel? Parent { get; set; }
    public uint ItemsAmount { get; set; }
}