using Telegram.WebUI.Models.Users;

namespace Telegram.WebUI.Models.Messages;

public class MessageViewModel
{
    public long Id { get; set; }

    public long SenderId { get; set; }
    public UserViewModel Sender { get; set; }

    public long ReceiverId { get; set; }
    public UserViewModel Receiver { get; set; }

    public long ChatRoomId { get; set; }

    public string Body { get; set; }

    public bool IsSeen { get; set; }

    public DateTime CreatedAt { get; set; }
}