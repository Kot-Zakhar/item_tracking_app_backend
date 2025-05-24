namespace Application.Common.ViewModels;

public class MovableItemViewModel
{
    public uint Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required CategoryViewModel Category { get; set; }
    public bool Visibility { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ImgSrc { get; set; }
}