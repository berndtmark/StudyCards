using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;
using StudyCards.Infrastructure.Database.Context;

namespace StudyCards.Infrastructure.Database.Repositories;

public class CardRepository : BaseRepository<Card>, ICardRepository
{
    private readonly DataBaseContext _dbContext;

    public CardRepository(DataBaseContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
    {
        _dbContext = dbContext;
    }

    public async Task<Card?> Get(Guid id)
    {
        return await _dbContext
            .Card
            .FindAsync(id);
    }

    public async Task<IEnumerable<Card>> GetByDeck(Guid deckId)
    {
        return await _dbContext.Card
            .Where(c => c.DeckId == deckId)
            .ToListAsync();
    }

    public async Task<Card> Add(Card card)
    {
        var entity = await AddEntity(card);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<Card> Update(Card card)
    {
        var entity = await UpdateEntity(card);
        await _dbContext.SaveChangesAsync();

        return entity;
    }
}

