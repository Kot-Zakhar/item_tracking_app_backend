using ItTrAp.InventoryService.DTOs.Categories;
using ItTrAp.InventoryService.Interfaces.Repositories;
using MediatR;

namespace ItTrAp.InventoryService.Queries.Categories;

public record GetDetailedCategoryTreeFromNodeQuery(uint Id) : IRequest<CategoryWithDetailsDto?>;

public class GetDetailedCategoryTreeFromNodeQueryHandler(
    ICategoryReadRepository categoryReadRepository) : IRequestHandler<GetDetailedCategoryTreeFromNodeQuery, CategoryWithDetailsDto?>
{
    public async Task<CategoryWithDetailsDto?> Handle(GetDetailedCategoryTreeFromNodeQuery request, CancellationToken cancellationToken)
    {
        return await categoryReadRepository.GetCategoryTreeFromNode(request.Id, cancellationToken);
    }
}