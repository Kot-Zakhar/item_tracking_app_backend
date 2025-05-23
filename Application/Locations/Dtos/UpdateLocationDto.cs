namespace Application.Locations.Dtos;

public struct UpdateLocationDto
{
    public sbyte? Floor { get; set; }
    public string? Name { get; set; } // it was Title
    public string? Department { get; set; }
}