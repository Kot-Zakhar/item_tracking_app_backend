using Domain.MovableItems;

namespace Domain.Categories;

public class Category
{
    public uint Id { get; set; }
    public required string Name { get; set; } // a change from 'title'
    public string? Icon { get; set; }

    public virtual Category? Parent { get; set; }
    public virtual List<Category> Children { get; set; } = new();
    public virtual List<MovableItem> MovableItems { get; set; } = new();
}