using Telegram.Domain.Common.Entities;

namespace Telegram.Domain.Entities;

public class Message : AuditableEntity
{
    public long SenderId { get; set; }
    public User Sender { get; set; }

    public long ReceiverId { get; set; }
    public User Receiver { get; set; }

    public long ChatRoomId { get; set; }
    public ChatRoom ChatRoom { get; set; }

    public string Body { get; set; }

    public bool IsSeen { get; set; }
}