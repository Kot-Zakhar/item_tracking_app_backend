namespace ItTrAp.LocationService.Application.DTOs;

public record LocationFiltersDto
{
    public string? Search { get; init; }
    public int? Top { get; init; }
    public sbyte? Floor { get; init; }
}