using System.Text.Json;
using System.Text.Json.Serialization;

namespace ItTrAp.InventoryService.DTOs.MovableItems;

public struct UpdateMovableItemDto
{
    public required string Name { get; set; }
    public uint CategoryId { get; set; }
    public string? Description { get; set; }
    public string? ImgSrc { get; set; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtraData { get; init; }
}