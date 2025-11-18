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

    public async Task<IEnumerable<Card>> GetCardsToStudy(Guid deckId, int noCardsToSelect, int noNewCardsToSelect, bool fillUnmet, CancellationToken cancellationToken = default)
    {
        if (noCardsToSelect == 0)
            return [];

        var now = DateTime.UtcNow;

        // Get IQuerable
        var cardsQuery = DbContext.Card
            .AsNoTracking()
            .WithPartitionKey(deckId);

        // 1. Select new cards (ReviewCount == 0) - executes as separate query
        var newCards = await cardsQuery
            .Where(card => card.CardReviewStatus.ReviewCount == 0)
            .OrderBy(card => card.CreatedDate)
            .Take(noNewCardsToSelect)
            .ToListAsync(cancellationToken); // Execute first query

        var newCardsCount = newCards.Count;
        var remainingSlots = noCardsToSelect - newCardsCount;

        if (remainingSlots <= 0)
        {
            return newCards;
        }

        // 2. Select due review cards - executes as separate query
        var newCardIds = newCards.Select(c => c.Id).ToHashSet();
        var reviewCards = await cardsQuery
            .Where(card =>
                card.CardReviewStatus.ReviewCount > 0 &&
                card.CardReviewStatus.NextReviewDate <= now &&
                !newCardIds.Contains(card.Id))
            .OrderBy(card => card.CardReviewStatus.NextReviewDate)
            .Take(remainingSlots)
            .ToListAsync(cancellationToken); // Execute second query

        var fillCount = remainingSlots - reviewCards.Count;
        if (!fillUnmet || fillCount <= 0)
        {
            return [.. newCards, .. reviewCards];
        }

        // 3. Fill with upcoming cards - executes as separate query
        var selectedIds = new HashSet<Guid>([.. newCardIds, .. reviewCards.Select(c => c.Id)]);
        var upcomingCards = await cardsQuery
            .Where(card => !selectedIds.Contains(card.Id))
            .OrderBy(card => card.CardReviewStatus.NextReviewDate)
            .Take(fillCount)
            .ToListAsync(cancellationToken); // Execute third query

        // 4. Combine and return
        return [.. newCards, .. reviewCards, .. upcomingCards];
    }
}

