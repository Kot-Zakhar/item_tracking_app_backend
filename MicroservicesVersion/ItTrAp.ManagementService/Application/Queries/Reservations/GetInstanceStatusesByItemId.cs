using ItTrAp.ManagementService.Application.DTOs.Reservations;
using ItTrAp.ManagementService.Application.Interfaces.Repositories;
using MediatR;

namespace ItTrAp.ManagementService.Application.Queries.Reservations;

public record GetInstanceStatusesByItemQuery(Guid ItemId)
    : IRequest<IList<InstanceStatusDto>>;

public class GetInstanceStatusesByItemQueryHandler(IReservationReadRepository repo) : IRequestHandler<GetInstanceStatusesByItemQuery, IList<InstanceStatusDto>>
{
    public async Task<IList<InstanceStatusDto>> Handle(GetInstanceStatusesByItemQuery request,
        CancellationToken cancellationToken)
    {
        return await repo.GetInstanceStatusesByItemIdAsync(request.ItemId, cancellationToken);
    }
}