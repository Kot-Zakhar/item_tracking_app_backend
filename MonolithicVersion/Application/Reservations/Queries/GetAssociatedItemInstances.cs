using Application.Reservations.DTOs;
using Application.Reservations.Interfaces;
using MediatR;

namespace Application.Reservations.Queries;

public record GetAssociatedItemInstancesQuery(uint UserId) : IRequest<IList<ItemInstanceDto>>;

public class GetAssociatedItemInstancesHandler(IReservationReadRepository reservationReadRepository) : IRequestHandler<GetAssociatedItemInstancesQuery, IList<ItemInstanceDto>>
{
    public async Task<IList<ItemInstanceDto>> Handle(GetAssociatedItemInstancesQuery request, CancellationToken cancellationToken)
    {
        return await reservationReadRepository.GetAssociatedItemInstancesAsync(request.UserId, cancellationToken);
    }
}