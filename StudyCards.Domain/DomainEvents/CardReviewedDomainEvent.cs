using StudyCards.Domain.Enums;
using StudyCards.Domain.Interfaces.DomainEvent;
using StudyCards.Domain.ValueObjects;

namespace StudyCards.Domain.DomainEvents;

public record CardReviewedDomainEvent(Guid CardId, ReviewPhase? PriorPhase, CardReviewStatus NewStatus) : IDomainEvent
{
}
