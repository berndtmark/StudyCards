using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Application.Interfaces.UnitOfWork;
using StudyCards.Domain.Entities;
using StudyCards.Domain.Interfaces.DomainEvent;
using StudyCards.Infrastructure.Database.Context;
using StudyCards.Infrastructure.Database.Repositories;

namespace StudyCards.Infrastructure.Database.UnitOfWork;

public sealed class UnitOfWork(DataBaseContext context, ICurrentUser currentUser, IDomainEventsDispatcher domainEventsDispatcher) : IUnitOfWork, IDisposable
{
    private ICardRepository? _cardRepository;
    private IDeckRepository? _deckRepository;
    private IUserRepository? _userRepository;
    private IStatisticRepository? _statisticsRepository;

    public ICardRepository CardRepository
    {
        get
        {
            _cardRepository ??= new CardRepository(context, currentUser);
            return _cardRepository;
        }
    }

    public IDeckRepository DeckRepository
    {
        get
        {
            _deckRepository ??= new DeckRepository(context, currentUser);
            return _deckRepository;
        }
    }

    public IUserRepository UserRepository
    {
        get
        {
            _userRepository ??= new UserRepository(context, currentUser);
            return _userRepository;
        }
    }

    public IStatisticRepository StatisticRepository
    {
        get
        {
            _statisticsRepository ??= new StatisticRepository(context, currentUser);
            return _statisticsRepository;
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

    public void Dispose()
    {
        context?.Dispose();
    }
}