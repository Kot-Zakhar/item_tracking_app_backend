namespace ItTrAp.QueryService.Application.Filters;

public record UserFiltersDto
{
    public string? Search { get; set; }
    public bool? HasInstances { get; set; }
}