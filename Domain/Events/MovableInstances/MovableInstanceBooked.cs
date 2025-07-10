namespace Domain.Events;

public record MovableInstanceBooked : DomainEvent
{
    public uint MovableInstanceId { get; }
    public Guid MovableInstanceCode { get; }
    public uint LocationId { get; }
    public Guid LocationCode { get; }
    public uint IssuerId { get; }
    public uint UserId { get; }

    public MovableInstanceBooked(
        uint movableInstanceId,
        Guid movableInstanceCode,
        uint locationId,
        Guid locationCode,
        uint issuerId,
        uint userId)
    {
        MovableInstanceId = movableInstanceId;
        MovableInstanceCode = movableInstanceCode;
        LocationId = locationId;
        LocationCode = locationCode;
        IssuerId = issuerId;
        UserId = userId;
    }
}