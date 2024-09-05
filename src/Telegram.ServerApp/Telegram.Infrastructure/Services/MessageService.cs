using System.Linq.Expressions;
using Telegram.Application.Services;
using Telegram.Domain.Entities;
using Telegram.Persistence.Repositories.Interfaces;

namespace Telegram.Infrastructure.Services;

public class MessageService(IMessageRepository messageRepository) : IMessageService
{
    public IEnumerable<Message> Get(Guid firstUserId, Expression<Func<Message, bool>>? predicate = null, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        var messages = messageRepository.Get(predicate, asNoTracking, cancellationToken);

        var task = Task.Run(async () =>
        {
            var seenMessages = messages.Where(seen => !seen.IsSeen && seen.ReceiverId == firstUserId).ToList();

            seenMessages.ForEach(message =>
            {
                message.IsSeen = true;
            });

            await messageRepository.UpdateRangeAsync(seenMessages, true, cancellationToken);
        });
        task.Wait();

        return messages.AsEnumerable();
    }

    public async ValueTask<Message?> GetByIdAsync(Guid id, Guid firstUserId, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        var message = await messageRepository.GetByIdAsync(id, asNoTracking, cancellationToken);

        if (message is not null && !message.IsSeen && message.ReceiverId == firstUserId)
        {
            message.IsSeen = true;
            await messageRepository.UpdateAsync(message, true, cancellationToken);
        }

        return message;
    }

    public ValueTask<IList<Message>> GetByUsersIdAsync(Guid firstUserId, Guid secondUserId, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        new(Get(firstUserId, message => (message.SenderId == firstUserId && message.ReceiverId == secondUserId)
            || (message.ReceiverId == firstUserId && message.SenderId == secondUserId), asNoTracking, cancellationToken).ToList());

    public ValueTask<Message> CreateAsync(Message entity, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        messageRepository.CreateAsync(entity, saveChanges, cancellationToken);

    public ValueTask<Message> UpdateAsync(Message entity, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        messageRepository.UpdateAsync(entity, saveChanges, cancellationToken);

    public ValueTask<Message> DeleteByIdAsync(Guid id, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        messageRepository.DeleteByIdAsync(id, saveChanges, cancellationToken);

    public ValueTask<bool> DeleteByChatIdAsync(Guid chatId, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        messageRepository.DeleteByChatIdAsync(chatId, saveChanges, cancellationToken);
}