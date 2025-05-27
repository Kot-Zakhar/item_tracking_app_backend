using Application.Common.ViewModels;
using Application.MovableInstances.Dtos;
using Application.MovableInstances.Interfaces;
using MediatR;

namespace Application.MovableInstances.Queries;

public record GetAllFilteredMovableInstancesQuery(uint ItemId, MovableInstanceFiltersDto Filters)
    : IRequest<List<MovableInstanceViewModel>>;

public class GetAllFilteredMovableInstancesQueryHandler(IMovableInstanceReadRepository repo) : IRequestHandler<GetAllFilteredMovableInstancesQuery, List<MovableInstanceViewModel>>
{
    public async Task<List<MovableInstanceViewModel>> Handle(GetAllFilteredMovableInstancesQuery request,
        CancellationToken cancellationToken)
    {
        return await repo.GetAllFilteredAsync(request.ItemId, request.Filters);
    }
}