using ItTrAp.InventoryService.Application.DTOs.Categories;
using ItTrAp.InventoryService.Infrastructure.Interfaces.Persistence.Repositories;
using MediatR;

namespace ItTrAp.InventoryService.Application.Queries.Categories;

public record GetDetailedCategoryTreeFromNodeQuery(uint Id) : IRequest<CategoryWithDetailsDto?>;

public class GetDetailedCategoryTreeFromNodeQueryHandler(
    ICategoryReadRepository categoryReadRepository) : IRequestHandler<GetDetailedCategoryTreeFromNodeQuery, CategoryWithDetailsDto?>
{
    public async Task<CategoryWithDetailsDto?> Handle(GetDetailedCategoryTreeFromNodeQuery request, CancellationToken cancellationToken)
    {
        return await categoryReadRepository.GetCategoryTreeFromNode(request.Id, cancellationToken);
    }
}