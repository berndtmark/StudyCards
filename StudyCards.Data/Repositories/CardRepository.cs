using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;
using StudyCards.Infrastructure.Database.Context;

namespace StudyCards.Infrastructure.Database.Repositories;

public class CardRepository(DataBaseContext dbContext, IHttpContextAccessor httpContextAccessor) : BaseRepository<Card>(dbContext, httpContextAccessor), ICardRepository
{
    private readonly DataBaseContext _dbContext = dbContext;

    public async Task<Card?> Get(Guid id, Guid deckId)
    {
        return await _dbContext
            .Card
            .WithPartitionKey(deckId)
            .AsNoTracking()
            .SingleOrDefaultAsync(d => d.Id == id);
    }

    public async Task<IEnumerable<Card>> GetByDeck(Guid deckId)
    {
        return await _dbContext.Card
            .AsNoTracking()
            .Where(c => c.DeckId == deckId)
            .ToListAsync();
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

    public async Task<int> CountByDeck(Guid deckId)
    {
        return await _dbContext.Card
            .AsNoTracking()
            .CountAsync(c => c.DeckId == deckId);
    }
}

