namespace Telegram.Api.Models.Dtos;

public class MessageDto
{
    public Guid Id { get; set; }

    public Guid SenderId { get; set; }

    public Guid ReceiverId { get; set; }

    public Guid ChatId { get; set; }

    public DateTimeOffset CreatedDate {  get; set; }

    public string Body { get; set; } = default!;

    public bool IsSeen { get; set; }
}