using System.Linq.Expressions;
using Telegram.Domain.Entities;

namespace Telegram.Persistence.Repositories.Interfaces;

public interface IMessageRepository
{
    IQueryable<Message> Get(Expression<Func<Message, bool>>? predicate = default, bool asNoTracking = false, CancellationToken cancellationToken = default);

    ValueTask<Message?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default);

    ValueTask<Message> CreateAsync(Message entity, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<Message> UpdateAsync(Message entity, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<IList<Message>> UpdateRangeAsync(IList<Message> entities, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<Message> DeleteByIdAsync(Guid entity, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<bool> DeleteByChatIdAsync(Guid chatId, bool saveChanges = true, CancellationToken cancellationToken = default);
}