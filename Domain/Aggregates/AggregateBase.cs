using Domain.Events;

namespace Domain.Aggregates;

public abstract class AggregateBase
{
    public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();

    protected void RaiseEvent(DomainEvent domainEvent)
    {
        DomainEvents.Add(domainEvent);
    }

    public void ClearEvents()
    {
        DomainEvents.Clear();
    }
}