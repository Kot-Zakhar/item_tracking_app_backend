using Application.Categories.Interfaces;
using MediatR;

namespace Application.Categories.Queries;

public record GetDetailedCategoryTreeQuery() : IRequest<List<CategoryWithDetailsViewModel>>;

public class GetDetailedCategoryTreeHandler(ICategoryReadRepository categoryRepository) : IRequestHandler<GetDetailedCategoryTreeQuery, List<CategoryWithDetailsViewModel>>
{
    public async Task<List<CategoryWithDetailsViewModel>> Handle(GetDetailedCategoryTreeQuery request, CancellationToken cancellationToken)
    {
        return await categoryRepository.GetCategoryTreeAsync(cancellationToken);
    }
}
