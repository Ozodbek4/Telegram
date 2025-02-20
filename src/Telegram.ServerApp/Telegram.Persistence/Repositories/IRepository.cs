using Telegram.Domain.Common.Entities;
using System.Linq.Expressions;

namespace Telegram.Persistence.Repositories;

public interface IRepository<TEntity> where TEntity : IAuditableEntity
{
    Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<TEntity?> SelectAsync(
        Expression<Func<TEntity, bool>> expression,
        string[]? includes = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> SelectAsEnumerableAsync(
        Expression<Func<TEntity, bool>>? expression = null,
        string[]? includes = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default);

    IQueryable<TEntity> SelectAsQueryable(
        Expression<Func<TEntity, bool>>? expression = null,
        string[]? includes = null,
        bool asNoTracking = true);
}