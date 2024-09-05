using System.Linq.Expressions;
using Telegram.Domain.Entities;

namespace Telegram.Application.Services;

public interface IMessageService
{
    IEnumerable<Message> Get(Expression<Func<Message, bool>>? predicate = default, bool asNoTracking = false, CancellationToken cancellationToken = default);

    ValueTask<Message?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default);

    ValueTask<IList<Message>> GetByUsersIdAsync(Guid firstUserId, Guid secondUserId, bool asNoTracking = false, CancellationToken cancellationToken = default);

    ValueTask<Message> CreateAsync(Message entity, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<Message> UpdateAsync(Message entity, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<Message> DeleteByIdAsync(Guid id, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<bool> DeleteByChatIdAsync(Guid chatId, bool saveChanges = true, CancellationToken cancellationToken = default);
}