using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;
using StudyCards.Infrastructure.Database.Context;

namespace StudyCards.Infrastructure.Database.Repositories;

public class DeckRepository(DataBaseContext dbContext, IHttpContextAccessor httpContextAccessor) : BaseRepository<Deck>(dbContext, httpContextAccessor), IDeckRepository
{
    public async Task<Deck?> Get(Guid id, string emailAddress, CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Deck
            .WithPartitionKey(emailAddress)
            .AsNoTracking()
            .SingleOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<Deck?> Get(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Deck
            .WithPartitionKey(EmailAddress)
            .AsNoTracking()
            .SingleOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Deck>> GetByEmail(string emailAddress, CancellationToken cancellationToken = default)
    {
        return await DbContext.Deck
            .AsNoTracking()
            .Where(c => c.UserEmail == emailAddress)
            .ToListAsync(cancellationToken);
    }

    // WARNING: This method returns all Decks, including those that do not belong to the current user.
    public async Task<IEnumerable<Deck>> GetDecksForAllUsers(CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Deck
            .AsNoTracking()
            .ToListAsync(cancellationToken);
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
