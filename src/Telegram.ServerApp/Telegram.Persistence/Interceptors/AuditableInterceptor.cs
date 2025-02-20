using Telegram.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Telegram.Persistence.Interceptors;

internal class AuditableInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var auditableEntry = eventData.Context!.ChangeTracker.Entries<IAuditableEntity>().ToList();

        auditableEntry.ForEach(entry =>
        {
            if (entry.State == EntityState.Added)
                entry.Property(nameof(IAuditableEntity.CreatedAt)).CurrentValue = DateTime.UtcNow;

            if (entry.State == EntityState.Modified)
                entry.Property(nameof(IAuditableEntity.UpdatedAt)).CurrentValue = DateTime.UtcNow;

            if (entry.State != EntityState.Deleted)
                return;

            entry.Property(nameof(IAuditableEntity.DeletedAt)).CurrentValue = DateTime.UtcNow;
            entry.Property(nameof(IAuditableEntity.IsDeleted)).CurrentValue = true;
            entry.State = EntityState.Modified;
        });

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}