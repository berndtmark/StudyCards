using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;
using StudyCards.Infrastructure.Database.Context;

namespace StudyCards.Infrastructure.Database.Repositories;

public class DeckRepository : BaseRepository<Deck>, IDeckRepository
{
    private readonly DataBaseContext _dbContext;

    public DeckRepository(DataBaseContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
    {
        _dbContext = dbContext;
    }

    public async Task<Deck?> Get(Guid id)
    {
        return await _dbContext
            .Deck
            .FindAsync(id);
    }

    public async Task<IEnumerable<Deck>> GetByEmail(string emailAddress)
    {
        return await _dbContext.Deck
            .Where(c => c.UserEmail == emailAddress)
            .ToListAsync();
    }

    public async Task<Deck> Add(Deck deck)
    {
        var entity = await AddEntity(deck);
        await _dbContext.SaveChangesAsync();

        return entity;
    }
}
