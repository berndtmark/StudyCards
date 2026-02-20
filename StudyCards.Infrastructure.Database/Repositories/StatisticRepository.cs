using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;
using StudyCards.Infrastructure.Database.Context;

namespace StudyCards.Infrastructure.Database.Repositories;

public class StatisticRepository(DataBaseContext dbContext, IHttpContextAccessor httpContextAccessor) : BaseRepository<Statistic>(dbContext, httpContextAccessor), IStatisticRepository
{
    public async Task<IEnumerable<T>> Get<T>(Guid userId, DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken = default) where T : Statistic
    {
        return await DbContext
            .Set<T>()
            .WithPartitionKey(userId)
            .Where(s => s.DateRecorded >= dateFrom && s.DateRecorded < dateTo)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> Get<T>(Guid userId, Guid deckId, CancellationToken cancellationToken = default) where T : Statistic
    {
        return await DbContext
            .Set<T>()
            .WithPartitionKey(userId)
            .Where(s => s.DeckId == deckId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Statistic> Add(Statistic statistic)
    {
        var entity = await AddEntity(statistic);
        return entity;
    }

    public Statistic Update(Statistic statistic)
    {
        var entity = UpdateEntity(statistic);
        return entity;
    }
}
