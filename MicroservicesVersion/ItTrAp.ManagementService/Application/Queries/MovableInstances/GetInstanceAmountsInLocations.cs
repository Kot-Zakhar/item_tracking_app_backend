using ItTrAp.ManagementService.Application.Interfaces.Repositories;
using MediatR;

namespace ItTrAp.ManagementService.Application.Queries.MovableInstances;

public record GetInstanceAmountsInLocationsQuery(List<uint> LocationIds)
    : IRequest<IList<uint>>;

public class GetInstanceAmountsInLocationsQueryHandler(IMovableInstanceReadRepository repo) : IRequestHandler<GetInstanceAmountsInLocationsQuery, IList<uint>>
{
    public async Task<IList<uint>> Handle(GetInstanceAmountsInLocationsQuery request,
        CancellationToken cancellationToken)
    {
        return await repo.GetInstanceAmountsInLocationsAsync(request.LocationIds, cancellationToken);
    }
}