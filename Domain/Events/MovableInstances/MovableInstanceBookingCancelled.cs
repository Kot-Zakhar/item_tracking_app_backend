namespace Domain.Events;

public record MovableInstanceBookingCancelled : DomainEvent
{
    public uint MovableInstanceId { get; }
    public Guid MovableInstanceCode { get; }
    public uint IssuerId { get; }
    public uint UserId { get; }

    public MovableInstanceBookingCancelled(
        uint movableInstanceId,
        Guid movableInstanceCode,
        uint issuerId,
        uint userId)
    {
        MovableInstanceId = movableInstanceId;
        MovableInstanceCode = movableInstanceCode;
        IssuerId = issuerId;
        UserId = userId;
    }
}