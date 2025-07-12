using ItTrAp.InventoryService.Interfaces;

namespace ItTrAp.InventoryService.Models;

public class MovableItem
{
    public uint Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool Visibility { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ImgSrc { get; set; }

    public virtual required Category Category { get; set; }

    public static async Task<MovableItem> CreateAsync(string name, string? description, string? imageUrl, Category category, IMovableItemUniquenessChecker uniquenessChecker, CancellationToken ct = default)
    {
        if (uniquenessChecker != null && !await uniquenessChecker.IsUniqueAsync(name, ct))
            throw new ArgumentException($"MovableItem with name '{name}' already exists.");

        return new MovableItem
        {
            Name = name,
            Description = description,
            ImgSrc = imageUrl,
            Category = category,
            CreatedAt = DateTime.UtcNow,
        };
    }

    public async Task UpdateAsync(string? name, string? description, string? imageUrl, Category? category, IMovableItemUniquenessChecker uniquenessChecker, CancellationToken ct = default)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            if (uniquenessChecker != null && !await uniquenessChecker.IsUniqueAsync(Id, name, ct))
                throw new ArgumentException($"MovableItem with name '{name}' already exists.");
            Name = name;
        }

        if (!string.IsNullOrWhiteSpace(description))
            Description = description;

        ImgSrc = imageUrl;

        if (category != null)
            Category = category;
    }
}
