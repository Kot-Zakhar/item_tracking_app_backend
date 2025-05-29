using Application.Common.DTOs;
using Application.MovableInstances.DTOs;
using Application.MovableInstances.Interfaces;
using MediatR;

namespace Application.MovableInstances.Queries;

public record GetAllFilteredMovableInstancesQuery(uint ItemId, MovableInstanceFiltersDto Filters)
    : IRequest<List<MovableInstanceDto>>;

public class GetAllFilteredMovableInstancesQueryHandler(IMovableInstanceReadRepository repo) : IRequestHandler<GetAllFilteredMovableInstancesQuery, List<MovableInstanceDto>>
{
    public async Task<List<MovableInstanceDto>> Handle(GetAllFilteredMovableInstancesQuery request,
        CancellationToken cancellationToken)
    {
        return await repo.GetAllFilteredAsync(request.ItemId, request.Filters);
    }
}