using Microsoft.Extensions.Logging;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.UseCases.Statistics.Commands;
using StudyCards.Domain.DomainEvents;
using StudyCards.Domain.Interfaces.DomainEvent;

namespace StudyCards.Application.EventHandlers;

public class StudyCompletedDomainEventHandler(ILogger<StudyCompletedDomainEventHandler> logger, ICurrentUser currentUser, ICQRSDispatcher dispatcher) : IDomainEventHandler<StudyCompletedDomainEvent>
{
    public async Task Handle(StudyCompletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId; // When Deck changes its SK to UserId, this will be become redundant

        var result = await dispatcher.Send(new AddStudyStatisticCommand
        {
            DeckId = domainEvent.DeckId,
            UserId = new Guid(userId),
            Name = domainEvent.DeckName,
            CardStudyCount = domainEvent.CardStudyCount
        });

        if (result.IsSuccess)
            logger.LogInformation("Study completed for deck {DeckName} with {CardStudyCount} cards studied. User: {UserId}", domainEvent.DeckName, domainEvent.CardStudyCount, userId);

        return;
    }
}
