namespace Telegram.Application.Services;

public interface IChatOrchestrationService
{
    Task<bool> MarkMessageAsSeenAsync(long chatRoomId, long userId, CancellationToken cancellationToken = default);
}