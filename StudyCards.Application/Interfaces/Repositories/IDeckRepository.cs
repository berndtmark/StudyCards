using StudyCards.Domain.Entities;

namespace StudyCards.Application.Interfaces.Repositories;

public interface IDeckRepository
{
    Task<Deck?> Get(Guid id);
    Task<IEnumerable<Deck>> GetByEmail(string emailAddress);
    Task<Deck> Add(Deck deck);
}
