using System.Linq.Expressions;
using Telegram.Domain.Entities;

namespace Telegram.Persistence.Repositories.Interfaces;

public interface IChatRepository
{
    IQueryable<Chat> Get(Expression<Func<Chat, bool>>? predicate = default, bool asNoTracking = false, CancellationToken cancellationToken = default);

    ValueTask<Chat?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default);

    ValueTask<Chat> CreateAsync(Chat entity, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<Chat> UpdateAsync(Chat entity, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<Chat> DeleteByIdAsync(Guid entity, bool saveChanges = true, CancellationToken cancellationToken = default);
}