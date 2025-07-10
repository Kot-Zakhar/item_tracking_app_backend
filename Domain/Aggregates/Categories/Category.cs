using Domain.Interfaces;

namespace Domain.Aggregates.Categories;

public class Category
{
    public uint Id { get; set; }
    public required string Name { get; set; }
    public string? Icon { get; set; }

    public virtual Category? Parent { get; set; }
    public virtual List<Category> Children { get; set; } = new();
    // public virtual List<MovableItem> MovableItems { get; set; } = new();

    public static async Task<Category> CreateAsync(string name, string? icon, Category? parent, ICategoryUniquenessChecker nameChecker, CancellationToken ct = default)
    {
        if (nameChecker != null && !await nameChecker.IsUniqueAsync(name, ct))
            throw new ArgumentException($"Category with name '{name}' already exists.");

        return new Category
        {
            Name = name,
            Icon = icon,
            Parent = parent,
        };
    }

    public async Task UpdateAsync(string? name, string? icon, ICategoryUniquenessChecker nameChecker, CancellationToken ct = default)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            if (nameChecker != null && !await nameChecker.IsUniqueAsync(Id, name, ct))
                throw new ArgumentException($"Category with name '{name}' already exists.");
            Name = name;
        }

        if (!string.IsNullOrWhiteSpace(icon))
            Icon = icon;
    }
}