using StudyCards.Domain.Entities;

namespace StudyCards.Application.Interfaces.Repositories;

public interface IDeckRepository
{
    Task<Deck?> Get(Guid id, string emailAddress, CancellationToken cancellationToken = default);
    Task<Deck?> Get(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Deck>> GetByEmail(string emailAddress, CancellationToken cancellationToken = default);
    // WARNING: This method returns all Decks, including those that do not belong to the current user.
    Task<IEnumerable<Deck>> GetDecksForAllUsers(CancellationToken cancellationToken = default);
    Task<Deck> Add(Deck deck);
    Deck Update(Deck deck);
    Task Remove(Guid id, string emailAddress);
}
