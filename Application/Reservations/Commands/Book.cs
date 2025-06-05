using Application.Reservations.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Reservations.Commands;

public record BookCommand(uint UserId, uint InstanceId) : IRequest;

public class BookCommandValidator : AbstractValidator<BookCommand>
{
    public BookCommandValidator()
    {
        RuleFor(x => (int)x.InstanceId).NotEqual(0).WithMessage("Instance ID must not be zero.");
        RuleFor(x => (int)x.UserId).NotEqual(0).WithMessage("User ID must not be zero.");
    }
}

public class BookCommandHandler(IReservationService reservationsService) : IRequestHandler<BookCommand>
{
    public async Task Handle(BookCommand request, CancellationToken cancellationToken)
    {
        await reservationsService.BookAsync(request.UserId, request.InstanceId, cancellationToken);
    }
}