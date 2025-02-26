namespace Telegram.WebUI.Models.Messages;

public class CreateMessageModel
{
    public long SenderId { get; set; }

    public long ReceiverId { get; set; }

    public string Body { get; set; }
}