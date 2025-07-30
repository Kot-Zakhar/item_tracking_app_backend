using ItTrAp.ManagementService.Application.Interfaces.Services;
using FluentValidation;
using MediatR;

namespace ItTrAp.ManagementService.Application.Commands.Reservations;

public record AssignCommand(uint IssuerId, uint AssigneeId, uint InstanceId) : IRequest;

public class AssignCommandValidator : AbstractValidator<AssignCommand>
{
    public AssignCommandValidator()
    {
        RuleFor(x => (int)x.IssuerId).NotEqual(0).WithMessage("User ID must not be zero.");
        RuleFor(x => (int)x.InstanceId).NotEqual(0).WithMessage("Instance ID must not be zero.");
        RuleFor(x => (int)x.AssigneeId).NotEqual(0).WithMessage("User ID must not be zero.");
    }
}

public class AssignCommandHandler(IReservationService reservationsService) : IRequestHandler<AssignCommand>
{
    public async Task Handle(AssignCommand request, CancellationToken ct)
    {
        await reservationsService.AssignAsync(request.IssuerId, request.AssigneeId, request.InstanceId, ct);
    }
}