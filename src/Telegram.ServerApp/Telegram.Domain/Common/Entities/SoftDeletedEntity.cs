namespace Telegram.Domain.Common.Entities;

public class SoftDeletedEntity : AuditableEntity, ISoftDeletedEntity
{
    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedDate { get; set; }
}