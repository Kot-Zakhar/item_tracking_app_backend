using ItTrAp.InventoryService.DTOs.Categories;
using ItTrAp.InventoryService.Interfaces.Repositories;
using MediatR;

namespace ItTrAp.InventoryService.Queries.Categories;

public record GetDetailedCategoryTreeQuery() : IRequest<List<CategoryWithDetailsDto>>;

public class GetDetailedCategoryTreeHandler(ICategoryReadRepository categoryRepository) : IRequestHandler<GetDetailedCategoryTreeQuery, List<CategoryWithDetailsDto>>
{
    public async Task<List<CategoryWithDetailsDto>> Handle(GetDetailedCategoryTreeQuery request, CancellationToken cancellationToken)
    {
        return await categoryRepository.GetCategoryTreeAsync(cancellationToken);
    }
}
