using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudyCards.Application.Common;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;
using StudyCards.Infrastructure.Database.Context;

namespace StudyCards.Infrastructure.Database.Repositories;

public class CardRepository(DataBaseContext dbContext, IHttpContextAccessor httpContextAccessor) : BaseRepository<Card>(dbContext, httpContextAccessor), ICardRepository
{
    public async Task<Card?> Get(Guid id, Guid deckId, CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Card
            .WithPartitionKey(deckId)
            .AsNoTracking()
            .SingleOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Card>?> Get(Guid[] ids, Guid deckId, CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Card
            .WithPartitionKey(deckId)
            .Where(c => ids.Contains(c.Id))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Card>> GetByDeck(Guid deckId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Card
            .AsNoTracking()
            .Where(c => c.DeckId == deckId)
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedResult<Card>> GetByDeck(Guid deckId, int pageNumber = 1, int pageSize = 10, string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        var query = DbContext.Card
            .AsNoTracking()
            .Where(c => c.DeckId == deckId);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            var loweredTerm = searchTerm.ToLowerInvariant();
            query = query.Where(c =>
                c.CardFront.ToLower().Contains(loweredTerm) ||
                c.CardBack.ToLower().Contains(loweredTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Card>(items, totalCount, pageNumber, pageSize);
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
        DbContext.Card.RemoveRange(cards);
    }

    public async Task<int> CountByDeck(Guid deckId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Card
            .AsNoTracking()
            .CountAsync(c => c.DeckId == deckId, cancellationToken);
    }

    public async Task<IEnumerable<Card>?> Search(Guid deckId, string searchTerm, CancellationToken cancellationToken = default)
    {
        var loweredTerm = searchTerm.ToLowerInvariant().Trim();

        return await DbContext
            .Card
            .WithPartitionKey(deckId)
            .AsNoTracking()
            .Where(c => c.CardFront.ToLower() == loweredTerm)
            .ToListAsync(cancellationToken);
    }
}

