using Microsoft.EntityFrameworkCore;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;
using StudyCards.Infrastructure.Database.Context;

namespace StudyCards.Infrastructure.Database.Repositories;

public class CardRepository(DataBaseContext dbContext) : ICardRepository
{
    public async Task<Card?> Get(Guid id)
    {
        return await dbContext
            .Card
            .FindAsync(id);
    }

    public async Task<IEnumerable<Card>> GetByDeck(Guid deckId)
    {
        return await dbContext.Card
            .Where(c => c.DeckId == deckId)
            .ToListAsync();
    }

    public async Task Add(Card card)
    {
        dbContext.Add(card);
        await dbContext.SaveChangesAsync();
    }

    public async Task Update(Card card)
    {
        var existingCard = await dbContext.Card.FindAsync(card.CardId);
        if (existingCard == null)
        {
            throw new Exception($"Card not found to update {card.CardId}");
        }

        dbContext.Entry(existingCard).CurrentValues.SetValues(card);
        await dbContext.SaveChangesAsync();
    }
}

