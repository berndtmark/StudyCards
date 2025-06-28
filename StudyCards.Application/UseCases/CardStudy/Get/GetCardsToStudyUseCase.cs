using Microsoft.AspNetCore.Http;
using StudyCards.Application.Exceptions;
using StudyCards.Application.Helpers;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;
using StudyCards.Domain.Interfaces;

namespace StudyCards.Application.UseCases.CardStudy.Get;

public class GetCardsToStudyUseCaseRequest
{
    public Guid DeckId { get; set; }
    public CardStudyMethodology StudyMethodology { get; set; }
}

public class GetCardsToStudyUseCase(
    IDeckRepository deckRepository, 
    ICardRepository cardRepository, 
    IHttpContextAccessor httpContextAccessor, 
    ICardStrategyContext cardStrategyContext, 
    ICardSelectionStudyFactory cardSelectionStudyFactory) : IUseCase<GetCardsToStudyUseCaseRequest, IEnumerable<Card>>
{
    public async Task<IEnumerable<Card>> Handle(GetCardsToStudyUseCaseRequest request)
    {
        var deck = await deckRepository.Get(request.DeckId, httpContextAccessor.GetEmail()) ?? throw new EntityNotFoundException(nameof(Deck), request.DeckId);
        var cards = await cardRepository.GetByDeck(request.DeckId);

        var cardStrategy = cardSelectionStudyFactory.Create(request.StudyMethodology);

        cardStrategyContext.SetStrategy(cardStrategy);
        cardStrategyContext.AddCards(cards);

        // Random strategy always lets you study cards
        var cardsToStudy = request.StudyMethodology switch
        {
            CardStudyMethodology.Random => Math.Min(deck.DeckSettings.ReviewsPerDay, deck.CardCount ?? deck.DeckSettings.ReviewsPerDay),
            _ => deck.CardNoToReview
        };    

        var result = cardStrategyContext.GetCards(cardsToStudy);

        return result;
    }
}
