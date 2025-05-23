using Domain.Common.Interfaces;
using Domain.Locations.Interfaces;
using Domain.MovableItems;

namespace Domain.Locations;

public class Location
{
    public uint Id { get; set; }
    public Guid Code { get; set; }
    public sbyte Floor { get; set; }
    public required string Name { get; set; }
    public string? Department { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual List<MovableInstance> Instances { get; set; } = new();

    public static async Task<Location> CreateAsync(string name, sbyte floor, string? department, INameUniquenessChecker<Location> nameChecker)
    {
        if (!await nameChecker.IsUniqueAsync(name))
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

    public async Task UpdateAsync(string? name, sbyte? floor, string? department, ILocationUniquenessChecker nameChecker)
    {

        if (!string.IsNullOrWhiteSpace(name))
        {
            if (!await nameChecker.IsUniqueAsync(name))
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