namespace ItTrAp.QueryService.Application.Common;

public record PaginatedFilteredQuery<FiltersDto> : PaginatedQuery
{
    public FiltersDto? Filters { get; set; }
}