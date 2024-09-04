using Telegram.Domain.Entities;

namespace Telegram.Application.Services;

public interface IChatOrchestrationService
{
    ValueTask<Message> SaveMessageToChatAsync(Message message, bool saveChanges = true, CancellationToken cancellationToken = default);
}