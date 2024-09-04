using Telegram.Domain.Entities;

namespace Telegram.Api.Models.Dtos;

public class ChatDto
{
    public Guid Id { get; set; }

    public Guid FirstUserId { get; set; }

    public Guid SecondUserId { get; set; }

    public Guid? LastMessageId { get; set; }

    public int FirstUserUnReadMessageCount { get; set; }

    public int SecondUserUnReadMessageCount { get; set; }

    public Message LastMessage { get; set; }

    public User FirstUser { get; set; }

    public User SecondUser { get; set; }
}