namespace StudyCards.Domain.Interfaces.DomainEvent;

public interface IDomainEventHandler<in T> where T : IDomainEvent
{
    Task Handle(T domainEvent, CancellationToken cancellationToken);
}
