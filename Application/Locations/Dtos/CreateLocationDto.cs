namespace Application.Locations.Dtos;

public struct CreateLocationDto
{
    public sbyte Floor { get; set; }
    public required string Name { get; set; } // it was Title
    public string? Department { get; set; }
}