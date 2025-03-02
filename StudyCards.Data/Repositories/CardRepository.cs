using Microsoft.EntityFrameworkCore;
using StudyCards.Data.Entities;
using StudyCards.Data.Interfaces;

namespace StudyCards.Data.Repositories;

public class CardRepository(Context.DataBaseContext dbContext) : ICardRepository
{
    public async Task<Card?> Get(Guid id)
    {
        return await dbContext
            .Cards
            .FindAsync(id);
    }

    public async Task Add(Card card)
    {
        dbContext.Add(card);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Card>> GetByEmail(string email)
    {
        return await dbContext.Cards
            .Where(c => c.UserEmail == email)
            .ToListAsync();
    }
}

