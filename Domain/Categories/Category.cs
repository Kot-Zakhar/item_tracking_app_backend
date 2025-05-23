using Domain.Common.Interfaces;
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

    public static async Task<Category> CreateAsync(string name, string? icon, Category? parent, INameUniquenessChecker<Category> nameChecker)
    {
        if (nameChecker != null && !await nameChecker.IsUniqueAsync(name))
            throw new ArgumentException($"Category with name '{name}' already exists.");

        return new Category
        {
            Name = name,
            Icon = icon,
            Parent = parent,
        };
    }

    public async Task UpdateAsync(string? name, string? icon, INameUniquenessChecker<Category> nameChecker)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            if (nameChecker != null && !await nameChecker.IsUniqueAsync(name))
                throw new ArgumentException($"Category with name '{name}' already exists.");
            Name = name;
        }

        if (!string.IsNullOrWhiteSpace(icon))
            Icon = icon;
    }
}