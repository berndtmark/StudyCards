using Microsoft.AspNetCore.Http;
using StudyCards.Application.Enums;
using StudyCards.Application.Helpers;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;

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
        var deck = await deckRepository.Get(request.DeckId, httpContextAccessor.GetEmail()) ?? throw new ArgumentException($"Deck with ID {request.DeckId} not found.");
        var cards = await cardRepository.GetByDeck(request.DeckId);

        var cardStrategy = cardSelectionStudyFactory.Create(request.StudyMethodology);

        cardStrategyContext.SetStrategy(cardStrategy);
        cardStrategyContext.AddCards(cards);

        var reviewsToday = Math.Min(deck.DeckSettings.ReviewsPerDay, deck.CardCount ?? deck.DeckSettings.ReviewsPerDay);

        // Random strategy always lets you study cards
        var cardsToStudy = request.StudyMethodology switch
        {
            CardStudyMethodology.Random => reviewsToday,
            _ => deck.DeckReviewStatus.LastReview.IsSameDay() ?
                    Math.Max(reviewsToday - deck.DeckReviewStatus.ReviewCount, 0) :
                    reviewsToday
        };    

        var result = cardStrategyContext.GetCards(cardsToStudy);

        return result;
    }
}
