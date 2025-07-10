namespace Domain.Events;

public record MovableInstanceMoved : DomainEvent
{
    public uint MovableInstanceId { get; }
    public Guid MovableInstanceCode { get; }
    public uint? OldLocationId { get; }
    public Guid? OldLocationCode { get; }
    public uint NewLocationId { get; }
    public Guid NewLocationCode { get; }
    public uint IssuerId { get; }

    public MovableInstanceMoved(
        uint movableInstanceId,
        Guid movableInstanceCode,
        uint? oldLocationId,
        Guid? oldLocationCode,
        uint newLocationId,
        Guid newLocationCode,
        uint issuerId)
    {
        MovableInstanceId = movableInstanceId;
        MovableInstanceCode = movableInstanceCode;
        OldLocationId = oldLocationId;
        OldLocationCode = oldLocationCode;
        NewLocationId = newLocationId;
        NewLocationCode = newLocationCode;
        IssuerId = issuerId;
    }
}