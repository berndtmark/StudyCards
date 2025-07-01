using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Infrastructure.Database.Context;
using Microsoft.AspNetCore.Http;
using StudyCards.Infrastructure.Database.Repositories;
using StudyCards.Application.Interfaces.UnitOfWork;

namespace StudyCards.Infrastructure.Database.UnitOfWork;

public class UnitOfWork(DataBaseContext context, IHttpContextAccessor httpContextAccessor) : IUnitOfWork
{
    private ICardRepository? _cardRepository;
    private IDeckRepository? _deckRepository;
    private bool _disposed;

    public ICardRepository CardRepository
    {
        get
        {
            _cardRepository ??= new CardRepository(context, httpContextAccessor);
            return _cardRepository;
        }
    }

    public IDeckRepository DeckRepository
    {
        get
        {
            _deckRepository ??= new DeckRepository(context, httpContextAccessor);
            return _deckRepository;
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}