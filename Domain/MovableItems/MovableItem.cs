namespace Domain.MovableItems;

using System;
using Domain.Categories;

public class MovableItem
{
    public uint Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool Visibility { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ImgSrc { get; set; }

    public virtual required Category Category { get; set; }
    public virtual List<MovableInstance> Instances { get; set; } = new();
}
