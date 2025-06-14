namespace Application.Categories.DTOs;

public struct CreateCategoryDto
{
    public required string Name { get; init; }
    public uint? ParentId { get; init; }
    public string? Icon { get; init; }
}