using ItTrAp.ManagementService.Application.DTOs.MovableInstances;
using ItTrAp.ManagementService.Application.Interfaces.Repositories;
using MediatR;

namespace ItTrAp.ManagementService.Application.Queries.MovableInstances;

public record GetAllFilteredMovableInstancesOfItemQuery(Guid ItemId, MovableInstanceFiltersDto Filters)
    : IRequest<List<MovableInstanceDto>>;

public class GetAllFilteredMovableInstancesOfItemQueryHandler(IMovableInstanceReadRepository repo) : IRequestHandler<GetAllFilteredMovableInstancesOfItemQuery, List<MovableInstanceDto>>
{
    public async Task<List<MovableInstanceDto>> Handle(GetAllFilteredMovableInstancesOfItemQuery request,
        CancellationToken cancellationToken)
    {
        return await repo.GetAllFilteredOfItemAsync(request.ItemId, request.Filters);
    }
}