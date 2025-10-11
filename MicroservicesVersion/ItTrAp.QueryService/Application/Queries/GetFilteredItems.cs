using ItTrAp.QueryService.Application.Filters;
using ItTrAp.QueryService.Application.Interfaces;
using ItTrAp.QueryService.Application.Common;
using ItTrAp.QueryService.Application.Responses;
using MediatR;

namespace ItTrAp.QueryService.Application.Queries;

public record GetFilteredItemsQuery() : PaginatedFilteredQuery<MovableItemFiltersDto>, IRequest<PaginatedResponse<MovableItemWithDetailsViewModel>>;

public class GetFilteredItemsQueryHandler(IQueryService queryService) : IRequestHandler<GetFilteredItemsQuery, PaginatedResponse<MovableItemWithDetailsViewModel>>
{
    public async Task<PaginatedResponse<MovableItemWithDetailsViewModel>> Handle(GetFilteredItemsQuery request, CancellationToken cancellationToken)
    {
        return await queryService.GetMovableItemsAsync(request, cancellationToken);
    }
}