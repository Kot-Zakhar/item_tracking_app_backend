using ItTrAp.QueryService.Application.Filters;
using ItTrAp.QueryService.Application.Interfaces;
using ItTrAp.QueryService.Application.Common;
using ItTrAp.QueryService.Application.Responses;
using MediatR;

namespace ItTrAp.QueryService.Application.Queries;

public record GetFilteredInstancesQuery(uint MovableItemId) : PaginatedFilteredQuery<MovableInstanceFiltersDto>, IRequest<PaginatedResponse<MovableInstanceViewModel>>;

public class GetFilteredInstancesQueryHandler(IQueryService queryService) : IRequestHandler<GetFilteredInstancesQuery, PaginatedResponse<MovableInstanceViewModel>>
{
    public async Task<PaginatedResponse<MovableInstanceViewModel>> Handle(GetFilteredInstancesQuery request, CancellationToken cancellationToken)
    {
        return await queryService.GetMovableInstancesAsync(request.MovableItemId, request, cancellationToken);
    }
}