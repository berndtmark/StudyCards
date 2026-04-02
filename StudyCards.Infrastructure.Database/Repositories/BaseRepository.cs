using Microsoft.EntityFrameworkCore;
using StudyCards.Application.Interfaces;
using StudyCards.Domain.Entities;
using StudyCards.Infrastructure.Database.Context;

namespace StudyCards.Infrastructure.Database.Repositories;

public abstract class BaseRepository<TEntity>(DataBaseContext dbContext, ICurrentUser currentUser) where TEntity : EntityBase
{
    protected virtual Guid UserId
    {
        get => field != Guid.Empty ? field : currentUser.UserId;
        set => field = value;
    }

    protected DataBaseContext DbContext => dbContext;

    #region Add
    protected virtual async Task<TEntity> AddEntity(TEntity entity, CancellationToken cancellationToken = default)
    {
        var newEntity = entity with
        {
            CreatedBy = UserId.ToString(),
        };

        await dbContext.AddAsync(newEntity, cancellationToken);
        return newEntity;
    }

    protected virtual async Task<IEnumerable<TEntity>> AddEntities(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        var userId = UserId.ToString();

        var newEntities = entities.Select(entity => entity with
        {
            CreatedBy = userId,
        }).ToList();

        await dbContext.AddRangeAsync(newEntities, cancellationToken);
        return newEntities;
    }
    #endregion Add

    #region Update
    protected virtual TEntity UpdateEntity(TEntity entity)
    {
        // Update Audit Fields
        var updateEntity = entity with
        {
            UpdatedDate = DateTime.UtcNow,
            UpdatedBy = UserId.ToString(),
        };

        dbContext.Update(updateEntity);
        return updateEntity;
    }
    #endregion Update

    #region Remove
    protected virtual void RemoveEntity(TEntity entity)
    {
        dbContext.Remove(entity);
    }

    protected virtual void RemoveEntity(IEnumerable<TEntity> entities)
    {
        dbContext.RemoveRange(entities);
    }

    protected virtual async Task RemoveEntity(Guid entityId, object? partitionKey = null)
    {
        var entity = await GetEntityById(entityId, partitionKey) ??
            throw new Exception($"{typeof(TEntity).FullName} not found to remove with Id:{entityId}");

        dbContext.Remove(entity);
    }
    #endregion Remove

    #region Methods
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
    #endregion Methods
}
