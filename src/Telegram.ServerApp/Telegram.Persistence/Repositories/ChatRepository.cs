using System.Linq.Expressions;
using Telegram.Domain.Common.Caching;
using Telegram.Domain.Entities;
using Telegram.Persistence.Caching.Brokers;
using Telegram.Persistence.DataContexts;
using Telegram.Persistence.Repositories.Interfaces;

namespace Telegram.Persistence.Repositories;

public class ChatRepository(TelegramDbContext context, ICacheBroker cacheBroker) 
    : EntityRepositoryBase<Chat, TelegramDbContext>(context, cacheBroker, new()), IChatRepository
{
    public new IQueryable<Chat> Get(Expression<Func<Chat, bool>>? predicate = null, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        base.Get(predicate, asNoTracking, cancellationToken);

    public new ValueTask<Chat?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        base.GetByIdAsync(id, asNoTracking, cancellationToken);

    public new ValueTask<Chat> CreateAsync(Chat entity, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        base.CreateAsync(entity, saveChanges, cancellationToken);

    public new ValueTask<Chat> DeleteByIdAsync(Guid entity, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        base.DeleteByIdAsync(entity, saveChanges, cancellationToken);

    public new ValueTask<Chat> UpdateAsync(Chat entity, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        base.UpdateAsync(entity, saveChanges, cancellationToken);
}