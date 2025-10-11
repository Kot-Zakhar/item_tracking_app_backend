namespace ItTrAp.QueryService.Application.Filters;

public record LocationFiltersDto
{
    public string? Search { get; init; }
    public int? Top { get; init; }
    public sbyte? Floor { get; init; }
    public bool? WithItemsOnly { get; init; }
}