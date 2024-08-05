using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Telegram.Domain.Common.Entities;

namespace Telegram.Persistence.Interceptors;

public class AuditableInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var auditableEntry = eventData.Context!.ChangeTracker.Entries<IAuditableEntity>().ToList();

        auditableEntry.ForEach(entry =>
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(nameof(IAuditableEntity.CreatedDate)).CurrentValue = DateTimeOffset.UtcNow;
                entry.Property(nameof(IAuditableEntity.Id)).CurrentValue = Guid.NewGuid();
            }

            if (entry.State == EntityState.Modified)
                entry.Property(nameof(IAuditableEntity.ModifiedDate)).CurrentValue = DateTimeOffset.UtcNow;
        });

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}