using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;
using StudyCards.Domain.Interfaces.DomainEvent;

namespace StudyCards.Domain.DomainEvents;

public record CardReviewedDomainEvent(Guid CardId, ReviewPhase? PriorPhase, CardReviewStatus NewStatus) : IDomainEvent
{
}
