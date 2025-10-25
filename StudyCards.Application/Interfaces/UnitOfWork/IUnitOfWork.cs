using StudyCards.Application.Interfaces.Repositories;

namespace StudyCards.Application.Interfaces.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    ICardRepository CardRepository { get; }
    IDeckRepository DeckRepository { get; }
    IUserRepository UserRepository { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
