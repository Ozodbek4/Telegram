using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Telegram.Application.Services;
using Telegram.Domain.Entities;
using Telegram.Persistence.Repositories.Interfaces;

namespace Telegram.Infrastructure.Services;

public class MessageService(IMessageRepository messageRepository) : IMessageService
{
    public IEnumerable<Message> Get(Expression<Func<Message, bool>>? predicate = null, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        messageRepository.Get(predicate, asNoTracking, cancellationToken);

    public ValueTask<Message?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        messageRepository.GetByIdAsync(id, asNoTracking, cancellationToken);

    public async ValueTask<IList<Message>> GetByUsersIdAsync(Guid firstUserId, Guid secondUserId, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        await messageRepository
        .Get(message => (message.SenderId == firstUserId && message.ReceiverId == secondUserId)
        || (message.ReceiverId == firstUserId && message.SenderId == secondUserId), asNoTracking, cancellationToken).ToListAsync();

    public ValueTask<Message> CreateAsync(Message entity, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        messageRepository.CreateAsync(entity, saveChanges, cancellationToken);

    public ValueTask<Message> UpdateAsync(Message entity, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        messageRepository.UpdateAsync(entity, saveChanges, cancellationToken);

    public ValueTask<Message> DeleteByIdAsync(Guid id, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        messageRepository.DeleteByIdAsync(id, saveChanges, cancellationToken);
}