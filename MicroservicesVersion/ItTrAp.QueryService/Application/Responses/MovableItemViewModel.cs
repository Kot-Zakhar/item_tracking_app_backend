namespace ItTrAp.QueryService.Application.Responses;

public class MovableItemViewModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required CategoryViewModel Category { get; set; }
    public bool Visibility { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ImgSrc { get; set; }
}