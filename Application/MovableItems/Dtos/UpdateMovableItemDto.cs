namespace Application.MovableItems.Dtos;

public struct UpdateMovableItemDto
{
    public string? Name { get; set; }
    public uint? CategoryId { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}