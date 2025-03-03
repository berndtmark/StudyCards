using StudyCards.Domain.Entities;

namespace StudyCards.Application.Interfaces.Repositories;

public interface ICardRepository
{
    Task<Card?> Get(Guid id);
    Task<IEnumerable<Card>> GetByEmail(string email);
    Task Add(Card card);
    Task Update(Card card);
}
