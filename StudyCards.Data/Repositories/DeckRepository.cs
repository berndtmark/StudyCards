using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;
using StudyCards.Infrastructure.Database.Context;

namespace StudyCards.Infrastructure.Database.Repositories;

public class DeckRepository(DataBaseContext dbContext, IHttpContextAccessor httpContextAccessor) : BaseRepository<Deck>(dbContext, httpContextAccessor), IDeckRepository
{
    private readonly DataBaseContext _dbContext = dbContext;

    public async Task<Deck?> Get(Guid id, string emailAddress)
    {
        return await _dbContext
            .Deck
            .WithPartitionKey(emailAddress)
            .AsNoTracking()
            .SingleOrDefaultAsync(d => d.Id == id);
    }

    public async Task<IEnumerable<Deck>> GetByEmail(string emailAddress)
    {
        return await _dbContext.Deck
            .AsNoTracking()
            .Where(c => c.UserEmail == emailAddress)
            .ToListAsync();
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

    public async Task Remove(Guid deckId, string emailAddress)
    {
        await RemoveEntity(deckId, emailAddress);
    }
}
