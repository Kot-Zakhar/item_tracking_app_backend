using ItTrAp.LocationService.Application.DTOs;
using ItTrAp.LocationService.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace ItTrAp.LocationService.Application.Commands;

public record CreateLocationCommand(CreateLocationDto Location) : IRequest<uint>;

public class CreateLocationCommandValidator : AbstractValidator<CreateLocationCommand>
{
    public CreateLocationCommandValidator()
    {
        RuleFor(x => x.Location)
            .NotNull()
            .WithMessage("Data is required.");

        RuleFor(x => x.Location.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name must be at most 100 characters long.");

        RuleFor(x => x.Location.Department)
            .MaximumLength(100)
            .WithMessage("Department must be at most 100 characters long.");
    }
}

public class CreateLocationHandler(ILocationService service) : IRequestHandler<CreateLocationCommand, uint>
{
    public Task<uint> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
        => service.CreateLocationAsync(request.Location, cancellationToken);
}