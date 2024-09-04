using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Telegram.Application.Services;
using Telegram.Domain.Entities;
using Telegram.Persistence.Repositories.Interfaces;

namespace Telegram.Infrastructure.Services;

public class ChatService(IChatRepository chatRepository) : IChatService
{
    public IEnumerable<Chat> Get(Expression<Func<Chat, bool>>? predicate = null, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        chatRepository.Get(predicate, asNoTracking, cancellationToken);

    public async ValueTask<Chat?> GetByUsersIdAsync(Guid firstUserId, Guid secondUserId, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        await chatRepository.Get(asNoTracking: asNoTracking, cancellationToken: cancellationToken)
            .FirstOrDefaultAsync(chat => (chat.FirstUserId == firstUserId && chat.SecondUserId == secondUserId)
            || (chat.SecondUserId == firstUserId && chat.FirstUserId == secondUserId), cancellationToken: cancellationToken);

    public async ValueTask<IList<Chat>> GetByUserIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        await chatRepository.Get(asNoTracking: asNoTracking, cancellationToken: cancellationToken)
            .Where(chat => chat.FirstUserId == id || chat.SecondUserId == id)
            .Include(chat => chat.FirstUser)
            .Include(chat => chat.SecondUser)
            .Include(chat => chat.LastMessage)
            .ToListAsync();

    public ValueTask<Chat?> GetByChatIdAsync(Guid chatId, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        chatRepository.GetByIdAsync(chatId, asNoTracking, cancellationToken);

    public ValueTask<Chat> CreateAsync(Guid firstUserId, Guid secondUserId, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        chatRepository.CreateAsync(new Chat { FirstUserId = firstUserId, SecondUserId =  secondUserId }, saveChanges, cancellationToken);

    public ValueTask<Chat> UpdateAsync(Chat entity, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        chatRepository.UpdateAsync(entity, saveChanges, cancellationToken);

    public ValueTask<Chat> DeleteByIdAsync(Guid id, bool saveChanges = true, CancellationToken cancellationToken = default) => 
        chatRepository.DeleteByIdAsync(id, saveChanges, cancellationToken);
}