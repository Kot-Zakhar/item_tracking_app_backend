using Domain.Interfaces;

namespace Domain.Models;

public class Location
{
    public uint Id { get; set; }
    public Guid Code { get; set; }
    public sbyte Floor { get; set; }
    public required string Name { get; set; }
    public string? Department { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual List<MovableInstance> Instances { get; set; } = new();

    public static async Task<Location> CreateAsync(string name, sbyte floor, string? department, ILocationUniquenessChecker nameChecker, CancellationToken ct = default)
    {
        if (!await nameChecker.IsUniqueAsync(name, ct))
            throw new ArgumentException($"Location with name '{name}' already exists.");

        return new Location
        {
            Code = Guid.NewGuid(),
            Name = name,
            Floor = floor,
            Department = department,
            CreatedAt = DateTime.UtcNow
        };
    }

    public async Task UpdateAsync(string? name, sbyte? floor, string? department, ILocationUniquenessChecker nameChecker, CancellationToken ct = default)
    {

        if (!string.IsNullOrWhiteSpace(name))
        {
            if (!await nameChecker.IsUniqueAsync(name, ct))
                throw new ArgumentException($"Location with name '{name}' already exists.");

            Name = name;
        }

        if (floor.HasValue)
        {
            Floor = floor.Value;
        }

        if (!string.IsNullOrWhiteSpace(department))
        {
            Department = department;
        }
    }
}