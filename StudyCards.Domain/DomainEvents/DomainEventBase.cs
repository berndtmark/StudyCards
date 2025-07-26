using StudyCards.Domain.Interfaces.DomainEvent;

namespace StudyCards.Domain.DomainEvents;

public abstract record DomainEventBase
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public List<IDomainEvent> DomainEvents => [.. _domainEvents];

    protected void Raise(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}
