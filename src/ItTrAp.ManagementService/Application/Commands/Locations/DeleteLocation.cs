using ItTrAp.ManagementService.Application.Interfaces.Services;
using MediatR;

namespace ItTrAp.ManagementService.Application.Commands.Locations;

public record DeleteLocationCommand(uint LocationId) : IRequest;

public class DeleteLocationCommandHandler(ILocationService locationService) : IRequestHandler<DeleteLocationCommand>
{
    public async Task Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
    {
        await locationService.DeleteAsync(request.LocationId, cancellationToken);
    }
}