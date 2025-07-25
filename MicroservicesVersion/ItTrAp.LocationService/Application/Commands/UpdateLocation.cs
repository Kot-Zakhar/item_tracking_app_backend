using ItTrAp.LocationService.Application.DTOs;
using ItTrAp.LocationService.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace ItTrAp.LocationService.Application.Commands;

public record UpdateLocationCommand(uint Id, UpdateLocationDto Location) : IRequest;

public class UpdateLocationCommandValidator : AbstractValidator<UpdateLocationCommand>
{
    public UpdateLocationCommandValidator()
    {
        RuleFor(x => (int)x.Id)
            .NotEqual(0)
            .WithMessage("Id must be greater than 0.");

        RuleFor(x => x.Location)
            .NotNull()
            .WithMessage("Data is required.");

        RuleFor(x => x.Location.Name)
            .MaximumLength(100)
            .WithMessage("Name must be at most 100 characters long.");

        RuleFor(x => x.Location.Department)
            .MaximumLength(100)
            .WithMessage("Department must be at most 100 characters long.");

        RuleFor(x => new { x.Location.Floor, x.Location.Name, x.Location.Department })
            .Must(fields => !(fields.Floor == null && string.IsNullOrEmpty(fields.Name) && string.IsNullOrEmpty(fields.Department)))
            .WithMessage("At least one of Floor, Name or Department must be provided.");
    }
}

public class UpdateLocationHandler(ILocationService service) : IRequestHandler<UpdateLocationCommand>
{
    public Task Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
        => service.UpdateLocationAsync(request.Id, request.Location, cancellationToken);
}