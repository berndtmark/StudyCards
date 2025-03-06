using Microsoft.EntityFrameworkCore;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;
using StudyCards.Infrastructure.Database.Context;

namespace StudyCards.Infrastructure.Database.Repositories;

public class DeckRepository(DataBaseContext dbContext) : IDeckRepository
{
    public async Task<Deck?> Get(Guid id)
    {
        return await dbContext
            .Deck
            .FindAsync(id);
    }

    public async Task<IEnumerable<Deck>> GetByEmail(string emailAddress)
    {
        return await dbContext.Deck
            .Where(c => c.UserEmail == emailAddress)
            .ToListAsync();
    }

    public async Task Add(Deck deck)
    {
        dbContext.Deck.Add(deck);
        await dbContext.SaveChangesAsync();
    }
}
