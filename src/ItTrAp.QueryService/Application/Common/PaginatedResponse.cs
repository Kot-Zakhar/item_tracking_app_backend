namespace ItTrAp.QueryService.Application.Common;

public class PaginatedResponse<T>
{
    public required IEnumerable<T> Payload { get; set; }
    public int TotalAmount { get; set; }
}