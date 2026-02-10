using ItTrAp.ManagementService.Application.Interfaces.Repositories;
using MediatR;

namespace ItTrAp.ManagementService.Application.Queries.Reservations;

public record GetItemAmountsByUserIdsQuery(List<uint> UserIds) : IRequest<IList<uint>>;

public class GetItemAmountsByUserIdsHandler : IRequestHandler<GetItemAmountsByUserIdsQuery, IList<uint>>
{
    private readonly IReservationReadRepository _reservationReadRepository;

    public GetItemAmountsByUserIdsHandler(IReservationReadRepository reservationReadRepository)
    {
        _reservationReadRepository = reservationReadRepository;
    }

    public async Task<IList<uint>> Handle(GetItemAmountsByUserIdsQuery request, CancellationToken cancellationToken)
    {
        return await _reservationReadRepository.GetItemAmountsByUserIdsAsync(request.UserIds, cancellationToken);
    }
}