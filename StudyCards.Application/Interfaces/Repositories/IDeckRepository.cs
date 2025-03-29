using StudyCards.Domain.Entities;

namespace StudyCards.Application.Interfaces.Repositories;

public interface IDeckRepository
{
    Task<Deck?> Get(Guid id, string emailAddress);
    Task<IEnumerable<Deck>> GetByEmail(string emailAddress);
    Task<Deck> Add(Deck deck);
    Task<Deck> Update(Deck deck);
    Task Remove(Guid id, string emailAddress);
}
