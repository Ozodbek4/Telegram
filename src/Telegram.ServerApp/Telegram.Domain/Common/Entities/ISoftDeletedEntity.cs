namespace Telegram.Domain.Common.Entities;

public interface ISoftDeletedEntity: IAuditableEntity
{
    bool IsDeleted { get; set; }

    DateTimeOffset? DeletedDate { get; set; }
}