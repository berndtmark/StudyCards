using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.UnitOfWork;

namespace StudyCards.Application.Services;

public class DeckCardCountService : IDeckCardCountService
{
    public async Task UpdateDeckCardCount(Guid deckId, IUnitOfWork unitOfWork)
    {
        var cardCount = await unitOfWork.CardRepository.CountByDeck(deckId);
        var deck = await unitOfWork.DeckRepository.Get(deckId) ?? throw new Exception($"Deck not found: {deckId}");
        deck = deck with
        {
            CardCount = cardCount
        };
        unitOfWork.DeckRepository.Update(deck);
    }
}
