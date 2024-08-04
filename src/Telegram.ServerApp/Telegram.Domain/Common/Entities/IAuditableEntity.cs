namespace Telegram.Domain.Common.Entities;

public interface IAuditableEntity : IEntity
{
    DateTimeOffset CreatedDate { get; set; }

    DateTimeOffset? ModifiedDate { get; set; }
}