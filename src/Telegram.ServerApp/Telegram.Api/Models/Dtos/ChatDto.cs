namespace Telegram.Api.Models.Dtos;

public class ChatDto
{
    public Guid Id { get; set; }

    public int FirstUserUnReadMessageCount { get; set; }

    public int SecondUserUnReadMessageCount { get; set; }

    public MessageDto LastMessage { get; set; }

    public UserDto FirstUser { get; set; }

    public UserDto SecondUser { get; set; }
}