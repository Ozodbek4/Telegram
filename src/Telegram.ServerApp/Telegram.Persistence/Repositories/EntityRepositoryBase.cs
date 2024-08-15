using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Telegram.Domain.Common.Caching;
using Telegram.Domain.Common.Entities;
using Telegram.Persistence.Caching.Brokers;

namespace Telegram.Persistence.Repositories;

public abstract class EntityRepositoryBase<TEntity, TContext>(TContext dbContext, ICacheBroker cacheBroker,
    CacheEntryOptions? cacheEntryOptions = default) 
        where TEntity : class, IEntity where TContext : DbContext
{
    protected TContext DbContext => dbContext;

    protected IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? predicate = default, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        var initialQuery = DbContext.Set<TEntity>().Where(entity => true);

        if (predicate is not null)
            initialQuery = initialQuery.Where(predicate);

        if (asNoTracking)
            initialQuery = initialQuery.AsNoTracking();

        return initialQuery;
    }

    protected async ValueTask<IList<TEntity>> GetAllAsync(bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        var initialQuery = DbContext.Set<TEntity>().Where(entity => true);

        if (asNoTracking)
            initialQuery = initialQuery.AsNoTracking();

        return await initialQuery.ToListAsync();
    }

    protected async ValueTask<TEntity?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        var foundEntity =  default(TEntity);

        if (cacheEntryOptions is null || !await cacheBroker.TryGetAsync(id.ToString(), out TEntity? cacheEntity))
        {
            var initialQuery = DbContext.Set<TEntity>().AsQueryable();
            if (asNoTracking)
                initialQuery = initialQuery.AsNoTracking();

            foundEntity = await initialQuery.FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
            if (foundEntity is not null && cacheEntryOptions is not null)
                await cacheBroker.SetAsync(foundEntity.Id.ToString(), foundEntity, cacheEntryOptions);
        }
        else
        {
            foundEntity = cacheEntity;
        }

        return foundEntity;
    }

    protected async ValueTask<IList<TEntity>> GetByIdsAsync(IList<Guid> ids, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        var initialQuery = DbContext.Set<TEntity>().Where(entity => true);

        if (asNoTracking)
            initialQuery = initialQuery.AsNoTracking();

        return await initialQuery.Where(entity => ids.Contains(entity.Id)).ToListAsync();
    }

    protected async ValueTask<TEntity> CreateAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<TEntity>().AddAsync(entity);

        if (saveChanges)
            await DbContext.SaveChangesAsync();

        if (cacheEntryOptions is not null)
            await cacheBroker.SetAsync(entity.Id.ToString(), entity, cacheEntryOptions);

        return entity;
    }

    protected async ValueTask<TEntity?> UpdateAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var result = await GetByIdAsync(entity.Id, true, cancellationToken);

        if (result is not null)
            DbContext.Update(entity);

        if (saveChanges)
            await DbContext.SaveChangesAsync();

        if (result is not null && cacheEntryOptions is not null)
            await cacheBroker.SetAsync(result.Id.ToString(), entity, cacheEntryOptions);

        return result is null ? null : entity;
    }

    protected async ValueTask<TEntity?> DeleteByIdAsync(Guid id, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var result = await GetByIdAsync(id, true, cancellationToken);

        if (result is not null)
            DbContext.Remove(result);

        if (saveChanges)
            await DbContext.SaveChangesAsync();

        if (result is not null && cacheEntryOptions is not null)
            await cacheBroker.DeleteAsync<TEntity>(id.ToString());

        return result;
    }
}