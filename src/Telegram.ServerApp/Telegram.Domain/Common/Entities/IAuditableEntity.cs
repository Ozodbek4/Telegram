namespace Telegram.Domain.Common.Entities;

public interface IAuditableEntity
{
    long Id { get; set; }

    DateTime CreatedAt { get; set; }

    DateTime? UpdatedAt { get; set; }

    DateTime? DeletedAt { get; set; }

    bool IsDeleted { get; set; }
}