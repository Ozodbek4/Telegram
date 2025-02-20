using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Telegram.Domain.Common.Entities;
using Telegram.Persistence.DataContexts;

namespace Telegram.Persistence.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IAuditableEntity
{
    private readonly AppDbContext _context;
    private readonly DbSet<TEntity> _set;

    public Repository(AppDbContext context)
    {
        _context = context;
        _set = _context.Set<TEntity>();
    }


    public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        (await _set.AddAsync(entity, cancellationToken)).Entity;

    public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        Task.FromResult(_set.Update(entity).Entity);

    public Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        Task.FromResult(_set.Remove(entity).Entity);

    public async Task<TEntity?> SelectAsync(
        Expression<Func<TEntity, bool>> expression,
        string[]? includes = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = _set.AsQueryable();

        if (includes is not null)
            foreach (var include in includes)
                query = query.Include(include);

        if (asNoTracking)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(expression, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> SelectAsEnumerableAsync(
        Expression<Func<TEntity, bool>>? expression = null,
        string[]? includes = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = expression is null ? _set : _set.Where(expression);

        if (includes is not null)
            foreach (var include in includes)
                query = query.Include(include);

        if (asNoTracking)
            query = query.AsNoTracking();

        return await query.ToListAsync(cancellationToken);
    }

    public IQueryable<TEntity> SelectAsQueryable(
        Expression<Func<TEntity, bool>>? expression = null,
        string[]? includes = null,
        bool asNoTracking = true)
    {
        var query = expression is null ? _set : _set.Where(expression);

        if (includes is not null)
            foreach (var include in includes)
                query = query.Include(include);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query;
    }
}