using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudyCards.Application.Helpers;
using StudyCards.Domain.Entities;
using StudyCards.Infrastructure.Database.Context;

namespace StudyCards.Infrastructure.Database.Repositories;

public abstract class BaseRepository<TEntity>(DataBaseContext dbContext, IHttpContextAccessor httpContextAccessor) where TEntity : EntityBase
{
    protected string EmailAddress => httpContextAccessor.GetEmail();

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

    protected virtual async Task<TEntity> AddEntity(TEntity entity)
    {
        var newEntity = entity with
        {
            CreatedBy = EmailAddress
        };

        await dbContext.AddAsync(newEntity);
        return newEntity;
    }

    protected virtual async Task RemoveEntity(Guid entityId)
    {
        var entity = await GetEntityById(entityId) ?? 
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
