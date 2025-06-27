using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;
using StudyCards.Infrastructure.Database.Context;

namespace StudyCards.Infrastructure.Database.Repositories;

public class CardRepository(DataBaseContext dbContext, IHttpContextAccessor httpContextAccessor) : BaseRepository<Card>(dbContext, httpContextAccessor), ICardRepository
{
    public async Task<Card?> Get(Guid id, Guid deckId, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Card
            .WithPartitionKey(deckId)
            .AsNoTracking()
            .SingleOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Card>> GetByDeck(Guid deckId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Card
            .AsNoTracking()
            .Where(c => c.DeckId == deckId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Card> Add(Card card)
    {
        var entity = await AddEntity(card);

        return entity;
    }

    public Card Update(Card card)
    {
        var entity = UpdateEntity(card);       
        return entity;
    }

    public async Task Remove(Guid id, Guid deckId)
    {
        await RemoveEntity(id, deckId);
    }

    public void RemoveRange(Card[] cards)
    {
        dbContext.Card.RemoveRange(cards);
    }

    public async Task<int> CountByDeck(Guid deckId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Card
            .AsNoTracking()
            .CountAsync(c => c.DeckId == deckId, cancellationToken);
    }
}

