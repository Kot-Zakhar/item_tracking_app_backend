using ItTrAp.ManagementService.Application.DTOs.MovableInstances;
using ItTrAp.ManagementService.Application.Interfaces.Repositories;
using MediatR;

namespace ItTrAp.ManagementService.Application.Queries.MovableInstances;

public record GetAllFilteredMovableInstancesQuery(MovableInstanceFiltersDto Filters)
    : IRequest<List<MovableInstanceDto>>;

public class GetAllFilteredMovableInstancesQueryHandler(IMovableInstanceReadRepository repo) : IRequestHandler<GetAllFilteredMovableInstancesQuery, List<MovableInstanceDto>>
{
    public async Task<List<MovableInstanceDto>> Handle(GetAllFilteredMovableInstancesQuery request,
        CancellationToken cancellationToken)
    {
        return await repo.GetAllFilteredAsync(request.Filters);
    }
}