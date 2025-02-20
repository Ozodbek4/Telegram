using Telegram.Domain.Common.Entities;

namespace Telegram.Domain.Entities;

public class ChatRoom : AuditableEntity
{
    public long FirstUserId { get; set; }
    public User FirstUser { get; set; }

    public long SecondUserId { get; set; }
    public User SecondUser { get; set; }

    public int FirstUserUnreadMessageCount { get; set; }
    public int SecondUserUnreadMessageCount { get; set; }

    public long? LastMessageId { get; set; }
    public Message? LastMessage { get; set; }

    public IEnumerable<Message> Messages { get; set; }
}