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
        
        var result = cardStrategyContext.GetCards(deck!.DeckSettings.ReviewsPerDay);

        return result;
    }
}
