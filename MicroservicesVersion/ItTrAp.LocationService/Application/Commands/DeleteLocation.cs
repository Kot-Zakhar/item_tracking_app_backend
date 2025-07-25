using ItTrAp.LocationService.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace ItTrAp.LocationService.Application.Commands;

public record DeleteLocationCommand(uint Id) : IRequest;

public class DeleteLocationCommandValidator : AbstractValidator<DeleteLocationCommand>
{
    public DeleteLocationCommandValidator()
    {
        RuleFor(x => (int)x.Id)
            .NotEqual(0)
            .WithMessage("Id must be greater than 0.");
    }
}

public class DeleteLocationCommandHandler(ILocationService locationService) : IRequestHandler<DeleteLocationCommand>
{
    public Task Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
        => locationService.DeleteLocationAsync(request.Id, cancellationToken);
}