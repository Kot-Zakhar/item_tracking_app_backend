namespace ItTrAp.LocationService.Application.DTOs;

public class LocationDto
{
    public uint Id { get; set; }
    public sbyte Floor { get; set; }
    public required string Name { get; set; }
    public required Guid Code { get; set; }
    public string? Department { get; set; }
    public DateTime CreatedAt { get; set; }
}