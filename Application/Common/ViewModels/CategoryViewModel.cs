namespace Application.Common.ViewModels;

public class CategoryViewModel
{
    public uint Id { get; set; }
    public required string Name { get; set; } // TODO: It was 'Title' on the FE
    public string? Icon { get; set; }
}
