using StudyCards.Domain.Interfaces.DomainEvent;

namespace StudyCards.Domain.DomainEvents;

public record StudyCompletedDomainEvent(Guid DeckId, Guid UserId, string DeckName, int CardStudyCount) : IDomainEvent
{
}
