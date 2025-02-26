using System.Linq.Expressions;
using Telegram.Application.Common.Exceptions;
using Telegram.Application.Services;
using Telegram.Domain.Entities;
using Telegram.Persistence.UnitOfWorks;

namespace Telegram.Infrastructure.Services;

public class MessageService(IUnitOfWork unitOfWork, IChatRoomService chatRoomService) : IMessageService
{
    public IQueryable<Message> Get(
        Expression<Func<Message, bool>>? expression = null,
        string[]? includes = null,
        bool asNoTracking = true
        )
    {
        return unitOfWork.Messages.SelectAsQueryable(expression, includes, asNoTracking);
    }

    public async Task<Message> GetByIdAsync(
        long id,
        string[]? includes = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        )
    {
        var exist = await unitOfWork.Messages.SelectAsync(entity => entity.Id == id && !entity.IsDeleted,
            includes, asNoTracking, cancellationToken)
            ?? throw new NotFoundException(nameof(Message), id);

        return exist;
    }

    public async Task<IEnumerable<Message>> GetByChatRoomIdAsync(
        long chatRoomId,
        string[]? includes = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        )
    {
        var exists = await unitOfWork.Messages
            .SelectAsEnumerableAsync(entity => entity.ChatRoomId == chatRoomId && !entity.IsDeleted,
                includes, asNoTracking, cancellationToken);

        return exists;
    }

    public async Task<Message> CreateAsync(Message message, CancellationToken cancellationToken = default)
    {
        var chatRoom = await chatRoomService.GetByUsersIdAsync(message.SenderId, message.ReceiverId, asNoTracking: false);

        if (chatRoom.FirstUserId == message.ReceiverId)
            chatRoom.FirstUserUnreadMessageCount++;
        else if (chatRoom.SecondUserId == message.ReceiverId)
            chatRoom.SecondUserUnreadMessageCount++;

        message.ChatRoomId = chatRoom.Id;
        var exist = await unitOfWork.Messages.CreateAsync(message, cancellationToken);

        chatRoom.LastMessage = exist;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return exist;
    }

    public async Task<Message> UpdateAsync(Message message, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.Messages.SelectAsync(entity => entity.Id == message.Id && !entity.IsDeleted,
            includes: ["Sender", "Receiver"],
            asNoTracking: false)
            ?? throw new NotFoundException(nameof(Message), message.Id);

        exist.Body = message.Body;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return exist;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.Messages.SelectAsync(entity => entity.Id == id && !entity.IsDeleted)
            ?? throw new NotFoundException(nameof(Message), id);

        await unitOfWork.Messages.DeleteAsync(exist, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}