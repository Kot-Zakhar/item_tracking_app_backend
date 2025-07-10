using MediatR;

namespace Domain.Events;

// Base interface for all domain events
public interface IDomainEvent : INotification
{
    Guid EventId { get; }
    DateTime OccurredAt { get; }
}

// User domain events
public record UserCreatedEvent(
    uint UserId,
    string Email,
    string FirstName,
    string LastName,
    string Phone
) : DomainEvent;

public record UserDeletedEvent(
    uint UserId,
    string Email
) : DomainEvent;

public record UserUpdatedEvent(
    uint UserId,
    string Email,
    string FirstName,
    string LastName,
    string Phone
) : DomainEvent;

// Inventory domain events
public record ItemCreatedEvent(
    uint ItemId,
    string Name,
    string Description,
    uint CategoryId,
    string CategoryName
) : DomainEvent;

public record ItemDeletedEvent(
    uint ItemId,
    string Name
) : DomainEvent;

public record InstanceCreatedEvent(
    Guid InstanceCode,
    uint ItemId,
    string ItemName,
    uint? LocationId,
    string? LocationName
) : DomainEvent;

public record InstanceDeletedEvent(
    Guid InstanceCode,
    uint ItemId,
    string ItemName
) : DomainEvent;

// Location domain events
public record LocationCreatedEvent(
    uint LocationId,
    Guid LocationCode,
    string Name,
    sbyte Floor,
    string? Department
) : DomainEvent;

public record LocationDeletedEvent(
    uint LocationId,
    Guid LocationCode,
    string Name
) : DomainEvent;

// Reservation domain events
public record InstanceBookedEvent(
    Guid InstanceCode,
    uint ItemId,
    string ItemName,
    uint UserId,
    string UserName,
    uint? LocationId,
    string? LocationName
) : DomainEvent;

public record InstanceTakenEvent(
    Guid InstanceCode,
    uint ItemId,
    string ItemName,
    uint UserId,
    string UserName,
    uint? PreviousLocationId,
    string? PreviousLocationName
) : DomainEvent;

public record InstanceReleasedEvent(
    Guid InstanceCode,
    uint ItemId,
    string ItemName,
    uint UserId,
    string UserName,
    uint LocationId,
    string LocationName
) : DomainEvent;

public record InstanceMovedEvent(
    Guid InstanceCode,
    uint ItemId,
    string ItemName,
    uint UserId,
    string UserName,
    uint? PreviousLocationId,
    string? PreviousLocationName,
    uint NewLocationId,
    string NewLocationName
) : DomainEvent;

public record BookingCancelledEvent(
    Guid InstanceCode,
    uint ItemId,
    string ItemName,
    uint UserId,
    string UserName,
    string Reason
) : DomainEvent;
