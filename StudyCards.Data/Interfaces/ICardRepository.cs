using StudyCards.Data.Entities;

namespace StudyCards.Data.Interfaces;

public interface ICardRepository
{
    Task<Card?> Get(Guid id);
    Task<IEnumerable<Card>> GetByEmail(string email);
    Task Add(Card card);
}
