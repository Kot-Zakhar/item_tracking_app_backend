using ItTrAp.ManagementService.Application.DTOs.Reservations;
using ItTrAp.ManagementService.Application.Interfaces.Repositories;
using MediatR;

namespace ItTrAp.ManagementService.Application.Queries.Reservations;

public record GetUserStatusesForItemsQuery(List<Guid> ItemIds) : IRequest<Dictionary<Guid, List<UserStatusDto>>>;

public class GetUserStatusesForItemsQueryHandler : IRequestHandler<GetUserStatusesForItemsQuery, Dictionary<Guid, List<UserStatusDto>>>
{
    private readonly IReservationReadRepository _reservationRepository;

    public GetUserStatusesForItemsQueryHandler(IReservationReadRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<Dictionary<Guid, List<UserStatusDto>>> Handle(GetUserStatusesForItemsQuery request, CancellationToken cancellationToken)
    {
        return await _reservationRepository.GetUserStatusesForItemsAsync(request.ItemIds, cancellationToken);
    }
}