using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Telegram.Domain.Common.Caching;
using Telegram.Domain.Common.Entities;
using Telegram.Persistence.Caching.Brokers;

namespace Telegram.Persistence.Repositories;

public abstract class EntityRepositoryBase<TEntity, TContext>(TContext dbContext, ICacheBroker cacheBroker,
    CacheEntryOptions? cacheEntryOptions = default)
        where TEntity : class, ISoftDeletedEntity where TContext : DbContext
{
    protected TContext DbContext => dbContext;

    protected IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? predicate = default, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        var initialQuery = DbContext.Set<TEntity>().Where(entity => !entity.IsDeleted);

        if (predicate is not null)
            initialQuery = initialQuery.Where(predicate);

        if (asNoTracking)
            initialQuery = initialQuery.AsNoTracking();

        return initialQuery;
    }

    protected async ValueTask<TEntity?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        var foundEntity = default(TEntity);

        if (cacheEntryOptions is null || !await cacheBroker.TryGetAsync(id.ToString(), out TEntity? cacheEntity))
        {
            var initialQuery = DbContext.Set<TEntity>().Where(entity => !entity.IsDeleted).AsQueryable();
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

    protected async ValueTask<TEntity> CreateAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

        if (saveChanges)
            await DbContext.SaveChangesAsync(cancellationToken);

        if (cacheEntryOptions is not null)
            await cacheBroker.SetAsync(entity.Id.ToString(), entity, cacheEntryOptions);

        return entity;
    }

    protected async ValueTask<TEntity> UpdateAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var result = await dbContext.Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(update => update.Id == entity.Id && !update.IsDeleted, cancellationToken) ??
            throw new ArgumentNullException();

        DbContext.Update(entity);

        if (saveChanges)
            await DbContext.SaveChangesAsync(cancellationToken);

        if (cacheEntryOptions is not null)
            await cacheBroker.SetAsync(entity.Id.ToString(), entity, cacheEntryOptions);

        return entity!;
    }

    protected async ValueTask<TEntity> DeleteByIdAsync(Guid id, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var result = await DbContext.Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == id && !entity.IsDeleted, cancellationToken) ??
            throw new ArgumentNullException("Entity is not exists");

        DbContext.Remove(result);

        if (saveChanges)
            await DbContext.SaveChangesAsync(cancellationToken);

        if (cacheEntryOptions is not null)
            await cacheBroker.DeleteAsync<TEntity>(id.ToString());

        return result!;
    }
}