using System.Text.Json;
using ItTrAp.InventoryService.DTOs.MovableItems;
using ItTrAp.InventoryService.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ItTrAp.InventoryService.Models;

public class MovableItem
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }

    [BsonElement("name")]
    public required string Name { get; set; }

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("categoryId")]
    public uint CategoryId { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("imgSrc")]
    [BsonIgnoreIfNull]
    public string? ImgSrc { get; set; }

    [BsonExtraElements]
    public BsonDocument? ExtraData { get; set; }

    [BsonIgnore]
    public virtual required Category Category { get; set; }

    public static async Task<MovableItem> CreateAsync(CreateMovableItemDto data, Category category, IMovableItemUniquenessChecker uniquenessChecker, CancellationToken ct = default)
    {
        if (uniquenessChecker != null && !await uniquenessChecker.IsUniqueAsync(data.Name, ct))
            throw new ArgumentException($"MovableItem with name '{data.Name}' already exists.");

        return new MovableItem
        {
            Id = Guid.NewGuid(),
            Name = data.Name,
            Description = data.Description,
            CategoryId = category.Id,
            Category = category,
            ImgSrc = data.ImgSrc,
            CreatedAt = DateTime.UtcNow,
            ExtraData = ConvertExtraDataToBson(data.ExtraData),
        };
    }

    public async Task UpdateAsync(UpdateMovableItemDto data, IMovableItemUniquenessChecker uniquenessChecker, CancellationToken ct = default)
    {
        if (uniquenessChecker != null && !await uniquenessChecker.IsUniqueAsync(Id, data.Name, ct))
            throw new ArgumentException($"MovableItem with name '{data.Name}' already exists.");
        Name = data.Name;
        Description = data.Description;
        ImgSrc = data.ImgSrc;
        CategoryId = data.CategoryId;
        ExtraData = ConvertExtraDataToBson(data.ExtraData);
    }
    
    private static BsonDocument? ConvertExtraDataToBson(IDictionary<string, JsonElement>? extraData)
    {
        if (extraData == null || extraData.Count == 0)
            return null;

        var bsonDoc = new BsonDocument();
        
        foreach (var kvp in extraData)
        {
            bsonDoc[kvp.Key] = ConvertJsonElementToBsonValue(kvp.Value);
        }
        
        return bsonDoc;
    }

    private static BsonValue ConvertJsonElementToBsonValue(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => BsonValue.Create(element.GetString()),
            JsonValueKind.Number => element.TryGetInt32(out var intVal) ? BsonValue.Create(intVal) :
                                element.TryGetInt64(out var longVal) ? BsonValue.Create(longVal) :
                                BsonValue.Create(element.GetDouble()),
            JsonValueKind.True => BsonValue.Create(true),
            JsonValueKind.False => BsonValue.Create(false),
            JsonValueKind.Null => BsonNull.Value,
            JsonValueKind.Object => BsonDocument.Parse(element.GetRawText()),
            JsonValueKind.Array => BsonArray.Create(element.EnumerateArray().Select(ConvertJsonElementToBsonValue)),
            _ => BsonDocument.Parse(element.GetRawText())
        };
    }
}
