using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudyCards.Application.Helpers;
using StudyCards.Domain.Entities;
using StudyCards.Infrastructure.Database.Context;

namespace StudyCards.Infrastructure.Database.Repositories;

public abstract class BaseRepository<TEntity>(DataBaseContext dbContext, IHttpContextAccessor httpContextAccessor) where TEntity : EntityBase
{
    protected virtual string EmailAddress
    {
        get => field ?? httpContextAccessor.GetEmail();
        set => field = value;
    }

    protected DataBaseContext DbContext => dbContext;

    protected virtual TEntity UpdateEntity(TEntity entity)
    {
        // Update Audit Fields
        var updateEntity = entity with
        {
            UpdatedDate = DateTime.UtcNow,
            UpdatedBy = EmailAddress
        };

        dbContext.Update<TEntity>(updateEntity);
        return updateEntity;
    }

    protected virtual async Task<TEntity> AddEntity(TEntity entity, CancellationToken cancellationToken = default)
    {
        var newEntity = entity with
        {
            CreatedBy = EmailAddress
        };

        await dbContext.AddAsync(newEntity, cancellationToken);
        return newEntity;
    }

    protected virtual async Task RemoveEntity(Guid entityId, object? partitionKey = null)
    {
        var entity = await GetEntityById(entityId, partitionKey) ?? 
            throw new Exception($"{typeof(TEntity).FullName} not found to remove with Id:{entityId}");

        dbContext.Remove(entity);
    }

    private async Task<TEntity?> GetEntityById(Guid entityId, object? partitionKey = null)
    {
        if (partitionKey != null)
        {
            return await dbContext.Set<TEntity>()
                .WithPartitionKey(partitionKey)
                .SingleOrDefaultAsync(d => d.Id == entityId);
        }
        else
        {
            return await dbContext.Set<TEntity>()
                .SingleOrDefaultAsync(d => d.Id == entityId);
        }
    }
}
