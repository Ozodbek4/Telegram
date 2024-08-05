using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Telegram.Domain.Common.Entities;

namespace Telegram.Persistence.Interceptors;

public class SoftDeletedInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var softDeletedEntry = eventData.Context!.ChangeTracker.Entries<ISoftDeletedEntity>().ToList();

        softDeletedEntry.ForEach(entry =>
        {
            if (entry.State != EntityState.Deleted)
                return;

            entry.Property(nameof(ISoftDeletedEntity.DeletedDate)).CurrentValue = DateTimeOffset.UtcNow;
            entry.Property(nameof(ISoftDeletedEntity.IsDeleted)).CurrentValue = true;
            entry.State = EntityState.Modified;
        });

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
