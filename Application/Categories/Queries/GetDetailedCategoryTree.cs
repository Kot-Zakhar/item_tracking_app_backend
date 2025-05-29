using Application.Categories.DTOs;
using Application.Categories.Interfaces;
using MediatR;

namespace Application.Categories.Queries;

public record GetDetailedCategoryTreeQuery() : IRequest<List<CategoryWithDetailsDto>>;

public class GetDetailedCategoryTreeHandler(ICategoryReadRepository categoryRepository) : IRequestHandler<GetDetailedCategoryTreeQuery, List<CategoryWithDetailsDto>>
{
    public async Task<List<CategoryWithDetailsDto>> Handle(GetDetailedCategoryTreeQuery request, CancellationToken cancellationToken)
    {
        return await categoryRepository.GetCategoryTreeAsync(cancellationToken);
    }
}
