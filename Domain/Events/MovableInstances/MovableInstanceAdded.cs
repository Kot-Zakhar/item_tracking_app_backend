namespace Domain.Events;

public record MovableInstanceAdded : DomainEvent
{
    public uint MovableInstanceId { get; }
    public Guid MovableInstanceCode { get; }
    public uint LocationId { get; }
    public Guid LocationCode { get; }
    public uint IssuerId { get; }

    public MovableInstanceAdded(
        uint movableInstanceId,
        Guid movableInstanceCode,
        uint locationId,
        Guid locationCode,
        uint issuerId)
    {
        MovableInstanceId = movableInstanceId;
        MovableInstanceCode = movableInstanceCode;
        LocationId = locationId;
        LocationCode = locationCode;
        IssuerId = issuerId;
    }
}