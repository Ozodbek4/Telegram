using Microsoft.EntityFrameworkCore;
using Telegram.Application.Common.Exceptions;
using Telegram.Application.Services;
using Telegram.Domain.Entities;
using Telegram.Persistence.UnitOfWorks;

namespace Telegram.Infrastructure.Services;

public class ChatOrchestrationService(IUnitOfWork unitOfWork) : IChatOrchestrationService
{
    public async Task<bool> MarkMessageAsSeenAsync(long chatRoomId, long userId, CancellationToken cancellationToken = default)
    {
        var chatRoom = await unitOfWork.ChatRooms.SelectAsync(entity => entity.Id == chatRoomId && !entity.IsDeleted,
            asNoTracking: false)
            ?? throw new NotFoundException(nameof(ChatRoom), chatRoomId);

        if (chatRoom.FirstUserId == userId)
            chatRoom.FirstUserUnreadMessageCount = 0;

        if (chatRoom.SecondUserId == userId)
            chatRoom.SecondUserUnreadMessageCount = 0;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var messages = unitOfWork.Messages.SelectAsQueryable(entity => entity.ChatRoomId == chatRoomId
            && entity.SenderId != userId && !entity.IsSeen,
            asNoTracking: false);

        await messages.ExecuteUpdateAsync(setters => setters.SetProperty(m => m.IsSeen, true), cancellationToken);

        return true;
    }
}