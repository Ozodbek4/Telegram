using Telegram.Domain.Common.Entities;

namespace Telegram.Domain.Entities;

public class Message : SoftDeletedEntity
{
    public Guid SenderId { get; set; }

    public Guid ReceiverId { get; set; }

    public Guid ChatId { get; set; }

    public string Body { get; set; } = default!;

    public bool IsSeen { get; set; }
}