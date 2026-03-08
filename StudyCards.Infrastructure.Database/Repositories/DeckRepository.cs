using Microsoft.EntityFrameworkCore;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;
using StudyCards.Infrastructure.Database.Context;

namespace StudyCards.Infrastructure.Database.Repositories;

public class DeckRepository(DataBaseContext dbContext, ICurrentUser currentUser) : BaseRepository<Deck>(dbContext, currentUser), IDeckRepository
{
    public async Task<Deck?> Get(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Deck
            .WithPartitionKey(userId)
            .AsNoTracking()
            .SingleOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<Deck?> Get(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Deck
            .WithPartitionKey(UserId)
            .AsNoTracking()
            .SingleOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Deck>> GetByUser(Guid userId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Deck
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Deck> Add(Deck deck)
    {
        var entity = await AddEntity(deck);
        return entity;
    }

    public Deck Update(Deck deck)
    {
        var entity = UpdateEntity(deck);
        return entity;
    }

    public async Task Remove(Guid deckId, Guid userId)
    {
        await RemoveEntity(deckId, userId);
    }
}
