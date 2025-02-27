using System.Linq.Expressions;
using Telegram.Application.Common.Exceptions;
using Telegram.Application.Common.Extensions;
using Telegram.Application.Common.Models;
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

    public Task<PaginationResult<Message>> GetByChatRoomIdAsync(
        long chatRoomId,
        PaginationParameters pagination,
        SortingParameters sorting,
        string? search = null,
        string[]? includes = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        )
    {
        var exists = unitOfWork.Messages
            .SelectAsQueryable(entity => entity.ChatRoomId == chatRoomId, includes, asNoTracking);

        if (search is not null)
            exists = exists.Where(entity => entity.Body.ToLower().Contains(search.ToLower()));

        exists = exists.Where(entity => !entity.IsDeleted).SortBy(sorting);

        return Task.FromResult(exists.ToPaginate(pagination));
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