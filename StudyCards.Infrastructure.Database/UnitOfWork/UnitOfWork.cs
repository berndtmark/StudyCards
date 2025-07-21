using MediatR;
using Microsoft.AspNetCore.Http;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Application.Interfaces.UnitOfWork;
using StudyCards.Domain.Entities;
using StudyCards.Infrastructure.Database.Context;
using StudyCards.Infrastructure.Database.Repositories;

namespace StudyCards.Infrastructure.Database.UnitOfWork;

public class UnitOfWork(DataBaseContext context, IHttpContextAccessor httpContextAccessor, IMediator mediator) : IUnitOfWork
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
        // before SaveChanges so its included in the same transaction
        // can be after too if that is needed
        await HandleDomainEvents(cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task HandleDomainEvents(CancellationToken cancellationToken)
    {
        var domainEvents = context.ChangeTracker.Entries<EntityBase>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }

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