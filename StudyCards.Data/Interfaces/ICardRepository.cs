using StudyCards.Data.Entities;

namespace StudyCards.Data.Interfaces;

public interface ICardRepository
{
    Task<Card?> Get(Guid id);
    Task Add(Card card);
}
