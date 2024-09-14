using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Telegram.Domain.Common.Caching;
using Telegram.Domain.Entities;
using Telegram.Persistence.Caching.Brokers;
using Telegram.Persistence.DataContexts;
using Telegram.Persistence.Repositories.Interfaces;

namespace Telegram.Persistence.Repositories;

public class MessageRepository
    : EntityRepositoryBase<Message, TelegramDbContext>, IMessageRepository
{
    private readonly TelegramDbContext _context;
    private readonly ICacheBroker _cacheBroker;

    public MessageRepository(TelegramDbContext context, ICacheBroker cacheBroker) : base(context, cacheBroker, new())
    {
        _context = context;
        _cacheBroker = cacheBroker;
    }

    public new IQueryable<Message> Get(Expression<Func<Message, bool>>? predicate = null, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        base.Get(predicate, asNoTracking, cancellationToken);

    public new ValueTask<Message?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        base.GetByIdAsync(id, asNoTracking, cancellationToken);

    public new ValueTask<Message> CreateAsync(Message entity, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        base.CreateAsync(entity, saveChanges, cancellationToken);

    public new ValueTask<Message> UpdateAsync(Message entity, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        base.UpdateAsync(entity, saveChanges, cancellationToken);

    public  async ValueTask<IList<Message>> UpdateRangeAsync(IList<Message> entities, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        _context.UpdateRange(entities);

        if (saveChanges)
            await _context.SaveChangesAsync(cancellationToken);

        entities.ToList().ForEach(async message =>
        {
            Message mes;

            if (await _cacheBroker.TryGetAsync(message.Id.ToString(), out mes!))
                await _cacheBroker.SetAsync(message.Id.ToString(), message);
        });
        
        return entities;
    }

    public new ValueTask<Message> DeleteByIdAsync(Guid entity, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        base.DeleteByIdAsync(entity, saveChanges, cancellationToken);

    public async ValueTask<bool> DeleteByChatIdAsync(Guid chatId, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var messages = await _context.Set<Message>().Where(message => !message.IsDeleted && message.ChatId == chatId).ToListAsync();

        messages.ForEach(async message =>
        {
            Message? isExists;
            message.IsDeleted = true;

            if (await _cacheBroker.TryGetAsync(message.Id.ToString(), out isExists))
                await _cacheBroker.DeleteAsync<Message>(chatId.ToString());
        });

        _context.UpdateRange(messages);

        if (saveChanges)
            await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}