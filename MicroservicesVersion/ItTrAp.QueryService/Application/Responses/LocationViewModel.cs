namespace ItTrAp.QueryService.Application.Responses;

public class LocationViewModel
{
    public uint Id { get; set; }
    public sbyte Floor { get; set; }
    public required string Name { get; set; }
    public string? Department { get; set; }
    public DateTime CreatedAt { get; set; }
}