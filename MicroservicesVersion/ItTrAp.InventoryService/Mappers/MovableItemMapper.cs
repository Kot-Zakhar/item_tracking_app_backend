using System.Text.Json;
using ItTrAp.InventoryService.DTOs.MovableItems;
using ItTrAp.InventoryService.Models;
using MongoDB.Bson;

namespace ItTrAp.InventoryService.Mappers;

public static class MovableItemMapper
{
    public static MovableItemDto ToDto(this MovableItem item)
    {
        return new MovableItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            CategoryId = item.CategoryId,
            CreatedAt = item.CreatedAt,
            ImgSrc = item.ImgSrc,
            ExtraData = ConvertBsonDocumentToJsonElementDictionary(item.ExtraData),
        };
    }

    public static MovableItemWithCategoryDto ToDtoWithCategory(this MovableItem item, Category category)
    {
        return new MovableItemWithCategoryDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Category = category.ToDto(),
            CategoryId = item.CategoryId,
            CreatedAt = item.CreatedAt,
            ImgSrc = item.ImgSrc,
            ExtraData = ConvertBsonDocumentToJsonElementDictionary(item.ExtraData),
        };
    }

    private static Dictionary<string, JsonElement>? ConvertBsonDocumentToJsonElementDictionary(BsonDocument? bsonDoc)
    {
        if (bsonDoc == null || bsonDoc.ElementCount == 0)
            return null;

        var result = new Dictionary<string, JsonElement>();
        
        foreach (var element in bsonDoc.Elements)
        {
            // Convert BsonValue to JSON string, then parse it into JsonElement
            var jsonString = element.Value.ToJson();
            result[element.Name] = JsonSerializer.Deserialize<JsonElement>(jsonString);
        }

        return result;
    }
}