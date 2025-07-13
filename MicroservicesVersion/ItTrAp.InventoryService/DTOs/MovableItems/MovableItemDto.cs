using System.Text.Json;
using System.Text.Json.Serialization;

namespace ItTrAp.InventoryService.DTOs.MovableItems;

public class MovableItemDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public uint CategoryId { get; set; }
    public bool Visibility { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ImgSrc { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraData { get; set; }
}