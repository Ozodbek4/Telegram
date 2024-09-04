using System.Linq.Expressions;
using Telegram.Domain.Common.Caching;
using Telegram.Domain.Entities;
using Telegram.Persistence.Caching.Brokers;
using Telegram.Persistence.DataContexts;
using Telegram.Persistence.Repositories.Interfaces;

namespace Telegram.Persistence.Repositories;

public class MessageRepository(TelegramDbContext context, ICacheBroker cacheBroker) 
    : EntityRepositoryBase<Message, TelegramDbContext>(context, cacheBroker, new ()), IMessageRepository
{
    public new IQueryable<Message> Get(Expression<Func<Message, bool>>? predicate = null, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        base.Get(predicate, asNoTracking, cancellationToken);

    public new ValueTask<Message?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        base.GetByIdAsync(id, asNoTracking, cancellationToken);

    public new ValueTask<Message> CreateAsync(Message entity, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        base.CreateAsync(entity, saveChanges, cancellationToken);

    public new ValueTask<Message> DeleteByIdAsync(Guid entity, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        base.DeleteByIdAsync(entity, saveChanges, cancellationToken);

    public new ValueTask<Message> UpdateAsync(Message entity, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        base.UpdateAsync(entity, saveChanges, cancellationToken);
}