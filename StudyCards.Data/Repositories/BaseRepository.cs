using Microsoft.AspNetCore.Http;
using StudyCards.Application.Helpers;
using StudyCards.Domain.Entities;
using StudyCards.Infrastructure.Database.Context;

namespace StudyCards.Infrastructure.Database.Repositories;

public abstract class BaseRepository<TEntity>(DataBaseContext dbContext, IHttpContextAccessor httpContextAccessor) where TEntity : EntityBase
{
    protected virtual async Task<TEntity> UpdateEntity(TEntity entity)
    {
        var existingCard = await dbContext.FindAsync<TEntity>(entity.Id);
        if (existingCard == null)
        {
            throw new Exception($"{typeof(TEntity).FullName} not found to update with Id:{entity.Id}");
        }

        // Update Audit Fields
        var updateEntity = entity with
        {
            UpdatedDate = DateTime.UtcNow,
            UpdatedBy = httpContextAccessor.GetEmail()
        };

        dbContext.Entry(existingCard).CurrentValues.SetValues(updateEntity);
        return updateEntity;
    }

    protected virtual async Task<TEntity> AddEntity(TEntity entity)
    {
        var newEntity = entity with
        {
            CreatedBy = httpContextAccessor.GetEmail()
        };

        await dbContext.AddAsync(newEntity);
        return newEntity;
    }

    protected virtual async Task RemoveEntity(Guid entityId)
    {
        var entity = await dbContext.FindAsync<TEntity>(entityId) ?? 
            throw new Exception($"{typeof(TEntity).FullName} not found to remove with Id:{entityId}");

        dbContext.Remove(entity);
    }
}
