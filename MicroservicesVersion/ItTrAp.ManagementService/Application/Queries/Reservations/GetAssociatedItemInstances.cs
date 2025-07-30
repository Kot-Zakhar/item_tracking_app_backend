using ItTrAp.ManagementService.Application.DTOs.MovableInstances;
using ItTrAp.ManagementService.Application.Interfaces.Repositories;
using MediatR;

namespace ItTrAp.ManagementService.Application.Queries.Reservations;

public record GetAssociatedItemInstancesQuery(uint UserId) : IRequest<IList<MovableInstanceDto>>;

public class GetAssociatedItemInstancesHandler(IReservationReadRepository reservationReadRepository) : IRequestHandler<GetAssociatedItemInstancesQuery, IList<MovableInstanceDto>>
{
    public async Task<IList<MovableInstanceDto>> Handle(GetAssociatedItemInstancesQuery request, CancellationToken cancellationToken)
    {
        return await reservationReadRepository.GetAssociatedItemInstancesAsync(request.UserId, cancellationToken);
    }
}