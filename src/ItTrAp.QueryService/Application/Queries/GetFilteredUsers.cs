using ItTrAp.QueryService.Application.Filters;
using ItTrAp.QueryService.Application.Interfaces;
using ItTrAp.QueryService.Application.Common;
using ItTrAp.QueryService.Application.Responses;
using MediatR;

namespace ItTrAp.QueryService.Application.Queries;

public record GetFilteredUsersQuery() : PaginatedFilteredQuery<UserFiltersDto>, IRequest<PaginatedResponse<UserWithDetailsViewModel>>;

public class GetFilteredUsersQueryHandler(IQueryService queryService) : IRequestHandler<GetFilteredUsersQuery, PaginatedResponse<UserWithDetailsViewModel>>
{
    public async Task<PaginatedResponse<UserWithDetailsViewModel>> Handle(GetFilteredUsersQuery request, CancellationToken cancellationToken)
    {
        return await queryService.GetUsersWithDetailsAsync(request, cancellationToken);
    }
}