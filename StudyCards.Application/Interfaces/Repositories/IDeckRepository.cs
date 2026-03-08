using StudyCards.Domain.Entities;

namespace StudyCards.Application.Interfaces.Repositories;

public interface IDeckRepository
{
    Task<Deck?> Get(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<Deck?> Get(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Deck>> GetByUser(Guid userId, CancellationToken cancellationToken = default);
    Task<Deck> Add(Deck deck);
    Deck Update(Deck deck);
    Task Remove(Guid id, Guid userId);
}
