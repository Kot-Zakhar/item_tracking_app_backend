using ItTrAp.InventoryService.Application.DTOs.MovableItems;
using ItTrAp.InventoryService.Infrastructure.Interfaces.Persistence.Repositories;
using MediatR;

namespace ItTrAp.InventoryService.Application.Queries.MovableItems;

public record GetMovableItemsByIdsQuery (IList<Guid> Ids) : IRequest<IList<MovableItemWithCategoryDto>>;

public class GetMovableItemsByIdsHandler(IMovableItemReadRepository movableItemReadRepository, ICategoryReadRepository categoryReadRepository)
    : IRequestHandler<GetMovableItemsByIdsQuery, IList<MovableItemWithCategoryDto>>
{
    public async Task<IList<MovableItemWithCategoryDto>> Handle(GetMovableItemsByIdsQuery request, CancellationToken cancellationToken)
    {
        var items = await movableItemReadRepository.GetByIdsAsync(request.Ids, cancellationToken);

        var categoryIds = items.Select(i => i.CategoryId).Distinct().ToList();

        var categories = await categoryReadRepository.GetByIdsAsync(categoryIds, cancellationToken);
        var categoryDict = categories.ToDictionary(c => c.Id, c => c);

        var result = items.Select(i => new MovableItemWithCategoryDto
        {
            Id = i.Id,
            Name = i.Name,
            Description = i.Description,
            CategoryId = i.CategoryId,
            Category = categoryDict[i.CategoryId], // TODO: Potential null reference exception
            ImgSrc = i.ImgSrc,
            CreatedAt = i.CreatedAt,
        }).ToList();

        return result;
    }
}