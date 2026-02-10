using ItTrAp.InventoryService.Application.DTOs.Categories;
using ItTrAp.InventoryService.Infrastructure.Interfaces.Persistence.Repositories;
using MediatR;

namespace ItTrAp.InventoryService.Application.Queries.Categories;

public record GetDetailedCategoryTreeQuery() : IRequest<List<CategoryWithDetailsDto>>;

public class GetDetailedCategoryTreeHandler(ICategoryReadRepository categoryRepository) : IRequestHandler<GetDetailedCategoryTreeQuery, List<CategoryWithDetailsDto>>
{
    public async Task<List<CategoryWithDetailsDto>> Handle(GetDetailedCategoryTreeQuery request, CancellationToken cancellationToken)
    {
        return await categoryRepository.GetCategoryTreeAsync(cancellationToken);
    }
}
