using System.Linq.Expressions;
using Telegram.Domain.Entities;

namespace Telegram.Application.Services;

public interface IChatService
{
    IEnumerable<Chat> Get(Expression<Func<Chat, bool>>? predicate = default, bool asNoTracking = false, CancellationToken cancellationToken = default);

    ValueTask<Chat?> GetByUsersIdAsync(Guid firstUserId, Guid secondUserId, bool asNoTracking = false, CancellationToken cancellationToken = default);

    ValueTask<IList<Chat>> GetByUserIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default);

    public ValueTask<Chat?> GetByChatIdAsync(Guid chatId, bool asNoTracking = false,  CancellationToken cancellationToken = default);

    ValueTask<Chat> CreateAsync(Guid firstUserId, Guid secondUserId, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<Chat> UpdateAsync(Chat entity, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<Chat> DeleteByIdAsync(Guid id, bool saveChanges = true, CancellationToken cancellationToken = default);
}