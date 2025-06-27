using StudyCards.Domain.Entities;

namespace StudyCards.Application.Interfaces.Repositories;

public interface ICardRepository
{
    Task<Card?> Get(Guid id, Guid deckId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Card>> GetByDeck(Guid deckId, CancellationToken cancellationToken = default);
    Task<Card> Add(Card card);
    Card Update(Card card);
    Task Remove(Guid id, Guid deckId);
    void RemoveRange(Card[] cards);
    Task<int> CountByDeck(Guid deckId, CancellationToken cancellationToken = default);
}
