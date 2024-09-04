using Telegram.Domain.Common.Entities;

namespace Telegram.Domain.Entities;

public class Chat : SoftDeletedEntity
{
    public Guid FirstUserId { get; set; }

    public Guid SecondUserId { get; set; }

    public Guid? LastMessageId { get; set; }
    
    public int FirstUserUnReadMessageCount { get; set; }

    public int SecondUserUnReadMessageCount { get; set; }

    public Message LastMessage { get; set; }

    public virtual User FirstUser { get; set; }

    public virtual User SecondUser { get; set; }
}