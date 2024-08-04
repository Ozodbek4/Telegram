using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Telegram.Domain.Common.Entities;

namespace Telegram.Persistence.Repositories;

public abstract class EntityRepositoryBase<TEntity, TContext>(TContext dbContext) where TEntity : class, IEntity where TContext : DbContext
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
        var initialQuery = dbContext.Set<TEntity>().Where(entity => true);

        if (asNoTracking)
            initialQuery = initialQuery.AsNoTracking();

        return await initialQuery.SingleOrDefaultAsync(entity => entity.Id == id);
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

        return entity;
    }

    protected async ValueTask<TEntity> UpdateAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        if (await GetById(entity.Id, true, cancellationToken) is not null)
            DbContext.Update(entity);

        return entity;
    }

    protected async ValueTask<TEntity?> DeleteByIdAsync(Guid id, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var result = await GetById(id, true, cancellationToken);

        if (result is not null)
            DbContext.Remove(result);

        return result;
    }
}