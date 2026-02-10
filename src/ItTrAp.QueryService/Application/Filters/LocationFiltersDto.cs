namespace ItTrAp.QueryService.Application.Filters;

public record LocationFiltersDto
{
    // TODO: it had Top field, which is replaced with PageSize in PaginatedFilteredQuery
    public string? Search { get; init; }
    public sbyte? Floor { get; init; }
    public bool? WithItemsOnly { get; init; }
}