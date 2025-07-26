using Microsoft.Extensions.Logging;
using StudyCards.Domain.DomainEvents;
using StudyCards.Domain.Interfaces.DomainEvent;

namespace StudyCards.Application.EventHandlers;

public class CardReviewedDomainEventHandler(ILogger<CardReviewedDomainEventHandler> logger) : IDomainEventHandler<CardReviewedDomainEvent>
{
    public Task Handle(CardReviewedDomainEvent notification, CancellationToken cancellationToken)
    {
        if (notification.PriorPhase != notification.NewStatus.CurrentPhase)
            logger.LogInformation("Card {CardId} phase changed from {PriorPhase} to {CurrentPhase}", notification.CardId, notification.PriorPhase, notification.NewStatus.CurrentPhase);

        return Task.CompletedTask;
    }
}
