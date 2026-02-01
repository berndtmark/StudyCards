using Microsoft.AspNetCore.Http;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Application.Interfaces.UnitOfWork;
using StudyCards.Domain.Entities;
using StudyCards.Domain.Interfaces.DomainEvent;
using StudyCards.Infrastructure.Database.Context;
using StudyCards.Infrastructure.Database.Repositories;

namespace StudyCards.Infrastructure.Database.UnitOfWork;

public class UnitOfWork(DataBaseContext context, IHttpContextAccessor httpContextAccessor, IDomainEventsDispatcher domainEventsDispatcher) : IUnitOfWork
{
    private ICardRepository? _cardRepository;
    private IDeckRepository? _deckRepository;
    private IUserRepository? _userRepository;
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

    public IUserRepository UserRepository
    {
        get
        {
            _userRepository ??= new UserRepository(context, httpContextAccessor);
            return _userRepository;
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);

        // before SaveChanges - included in the same transaction
        // after SaveChanges - separate transaction
        await HandleDomainEvents(cancellationToken);
    }

    private async Task HandleDomainEvents(CancellationToken cancellationToken)
    {
        var domainEvents = context.ChangeTracker.Entries<EntityBase>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        await domainEventsDispatcher.DispatchAsync(domainEvents, cancellationToken);

        foreach (var entity in context.ChangeTracker.Entries<EntityBase>())
        {
            entity.Entity.ClearDomainEvents();
        }
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