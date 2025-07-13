using System.Text.Json.Serialization;

namespace ItTrAp.InventoryService.DTOs.Categories;

public struct UpdateCategoryDto
{
    public string? Name { get; init; }
    public string? Icon { get; init; }

    [JsonExtensionData]
    public Dictionary<string, object>? ExtraData { get; init; }
}