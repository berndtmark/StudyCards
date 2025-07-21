using MediatR;

namespace StudyCards.Domain.Interfaces.DomainEvent;

public interface IDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
}
