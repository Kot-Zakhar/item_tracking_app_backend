using Application.Categories.DTOs;
using Application.Categories.Interfaces;
using MediatR;

namespace Application.Categories.Queries;

public record GetDetailedCategoryTreeFromNodeQuery(uint Id) : IRequest<CategoryWithDetailsDto?>;

public class GetDetailedCategoryTreeFromNodeQueryHandler(
    ICategoryReadRepository categoryReadRepository) : IRequestHandler<GetDetailedCategoryTreeFromNodeQuery, CategoryWithDetailsDto?>
{
    public async Task<CategoryWithDetailsDto?> Handle(GetDetailedCategoryTreeFromNodeQuery request, CancellationToken cancellationToken)
    {
        return await categoryReadRepository.GetCategoryTreeFromNode(request.Id, cancellationToken);
    }
}