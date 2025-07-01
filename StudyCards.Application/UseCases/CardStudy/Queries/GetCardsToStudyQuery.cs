using Microsoft.AspNetCore.Http;
using StudyCards.Application.Exceptions;
using StudyCards.Application.Helpers;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;
using StudyCards.Domain.Interfaces;

namespace StudyCards.Application.UseCases.CardStudy.Queries;

public class GetCardsToStudyQuery : IQuery<IEnumerable<Card>>
{
    public Guid DeckId { get; set; }
    public CardStudyMethodology StudyMethodology { get; set; }
}

public class GetCardsToStudyQueryHandler(
    IDeckRepository deckRepository, 
    ICardRepository cardRepository, 
    IHttpContextAccessor httpContextAccessor, 
    ICardsToStudyStrategyContext cardStrategyContext, 
    ICardSelectionStudyFactory cardSelectionStudyFactory) : IQueryHandler<GetCardsToStudyQuery, IEnumerable<Card>>
{
    public async Task<IEnumerable<Card>> Handle(GetCardsToStudyQuery request, CancellationToken cancellationToken)
    {
        var deck = await deckRepository.Get(request.DeckId, httpContextAccessor.GetEmail(), cancellationToken) ?? throw new EntityNotFoundException(nameof(Deck), request.DeckId);
        var cards = await cardRepository.GetByDeck(request.DeckId, cancellationToken);

        var cardStrategy = cardSelectionStudyFactory.Create(request.StudyMethodology);

        cardStrategyContext.SetStrategy(cardStrategy);
        cardStrategyContext.AddCards(cards);

        // Random strategy always lets you study cards
        var cardsToStudy = request.StudyMethodology switch
        {
            CardStudyMethodology.Random => Math.Min(deck.DeckSettings.ReviewsPerDay, deck.CardCount ?? deck.DeckSettings.ReviewsPerDay),
            _ => deck.CardNoToReview
        };    

        var result = cardStrategyContext.GetStudyCards(cardsToStudy, deck.DeckSettings.NewCardsPerDay);

        return result;
    }
}
