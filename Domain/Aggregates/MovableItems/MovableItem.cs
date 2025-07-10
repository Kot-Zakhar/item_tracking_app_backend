using Domain.Aggregates.Categories;
using Domain.Aggregates.Locations;
using Domain.Aggregates.MovableInstances;
using Domain.Aggregates.Users;
using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Aggregates.MovableItems;

public class MovableItem
{
    public uint Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool Visibility { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ImgSrc { get; set; }

    public virtual required Category Category { get; set; }
    public virtual List<MovableInstance> Instances { get; set; } = new();

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

    public MovableInstance QuickAddInstance(User issuer)
    {
        if (issuer == null) throw new ArgumentNullException(nameof(issuer));

        var instance = MovableInstance.Create(this, issuer);

        Instances.Add(instance);

        return instance;
    }

    public MovableInstance BookAnyInstanceInLocation(
        User issuer,
        Location location,
        User user)
    {
        if (location == null) throw new ArgumentNullException(nameof(location));
        if (user == null) throw new ArgumentNullException(nameof(user));

        var availableInstance = Instances
            .Where(x => x.Location?.Id == location.Id && x.Status == MovableInstanceStatus.Available)
            .FirstOrDefault();

        if (availableInstance == null)
        {
            throw new InvalidOperationException("No available instance found in the specified location.");
        }

        availableInstance.Book(issuer, user);

        return availableInstance;
    }
}
