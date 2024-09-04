using Telegram.Application.Services;
using Telegram.Domain.Entities;

namespace Telegram.Infrastructure.Services;

public class ChatOrchestrationService(IMessageService messageService, IChatService chatService) : IChatOrchestrationService
{
    public async ValueTask<Message> SaveMessageToChatAsync(Message message, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var chat = await chatService.GetByUsersIdAsync(message.SenderId, message.ReceiverId, true, cancellationToken);

        if (chat is null)
            chat = await chatService.CreateAsync(message.SenderId, message.ReceiverId, saveChanges, cancellationToken);

        if (!message.IsSeen && message.ReceiverId == chat.FirstUserId)
            chat.FirstUserUnReadMessageCount++;

        if (!message.IsSeen && message.ReceiverId == chat.SecondUserId)
            chat.SecondUserUnReadMessageCount++;

        message.ChatId = chat.Id;

        var newMessage = await messageService.CreateAsync(message, saveChanges, cancellationToken);
        chat.LastMessageId = newMessage.Id;
        
        await chatService.UpdateAsync(chat);

        return newMessage;
    }
}