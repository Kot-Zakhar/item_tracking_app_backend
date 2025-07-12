namespace ItTrAp.InventoryService.DTOs.Categories;

public class CategoryTreeNodeDto<NodeType> : CategoryDto where NodeType : CategoryTreeNodeDto<NodeType>
{
    public List<NodeType>? Children { get; set; }
    public NodeType? Parent { get; set; }
}