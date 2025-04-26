using StudyCards.Domain.Entities;

namespace StudyCards.Application.Interfaces.Repositories;

public interface ICardRepository
{
    Task<Card?> Get(Guid id, Guid deckId);
    Task<IEnumerable<Card>> GetByDeck(Guid deckId);
    Task<Card> Add(Card card);
    Card Update(Card card);
    Task Remove(Guid id, Guid deckId);
}
