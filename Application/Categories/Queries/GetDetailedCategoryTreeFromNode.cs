using Application.Categories.Interfaces;
using MediatR;

namespace Application.Categories.Queries;

public record GetDetailedCategoryTreeFromNodeQuery(uint Id) : IRequest<CategoryWithDetailsViewModel?>;

public class GetDetailedCategoryTreeFromNodeQueryHandler(
    ICategoryReadRepository categoryReadRepository) : IRequestHandler<GetDetailedCategoryTreeFromNodeQuery, CategoryWithDetailsViewModel?>
{
    public async Task<CategoryWithDetailsViewModel?> Handle(GetDetailedCategoryTreeFromNodeQuery request, CancellationToken cancellationToken)
    {
        return await categoryReadRepository.GetCategoryTreeFromNode(request.Id, cancellationToken);
    }
}