using Telegram.WebUI.Models.Messages;
using Telegram.WebUI.Models.Users;

namespace Telegram.WebUI.Models.ChatRooms;

public class ChatRoomViewModel
{
    public long Id { get; set; }

    public long FirstUserId { get; set; }
    public UserViewModel FirstUser { get; set; }

    public long SecondUserId { get; set; }
    public UserViewModel SecondUser { get; set; }

    public int FirstUserUnreadMessageCount { get; set; }
    public int SecondUserUnreadMessageCount { get; set; }

    public long? LastMessageId { get; set; }
    public MessageViewModel? LastMessage { get; set; }
}