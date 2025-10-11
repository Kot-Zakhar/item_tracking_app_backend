namespace ItTrAp.QueryService.Application.Common;

public record PaginatedFilteredQuery<FiltersDto> : PaginatedQuery
{
    public required FiltersDto Filters { get; set; }
}