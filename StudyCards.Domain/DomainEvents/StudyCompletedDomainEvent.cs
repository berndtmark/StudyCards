using StudyCards.Domain.Interfaces.DomainEvent;

namespace StudyCards.Domain.DomainEvents;

public record StudyCompletedDomainEvent(Guid DeckId, string DeckName, int CardStudyCount) : IDomainEvent
{
}
