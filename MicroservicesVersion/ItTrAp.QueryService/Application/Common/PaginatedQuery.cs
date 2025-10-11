namespace ItTrAp.QueryService.Application.Common;

public record PaginatedQuery
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}