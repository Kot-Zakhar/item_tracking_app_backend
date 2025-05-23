namespace Application.Categories.Interfaces;

public interface ICategoryReadRepository
{
    // Task<CategoryWithDetailsViewModel?> GetByIdAsync(int id, CancellationToken ct = default);
    // Task<List<CategoryWithDetailsViewModel>> GetAllFiltered(string? search, int? top, CancellationToken ct = default);
    Task<List<CategoryWithDetailsViewModel>> GetCategoryTreeAsync(CancellationToken ct = default);
    Task<CategoryWithDetailsViewModel?> GetCategoryTreeFromNode(uint id, CancellationToken ct = default);
}