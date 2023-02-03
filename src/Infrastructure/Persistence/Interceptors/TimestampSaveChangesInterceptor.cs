using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence.Interceptors;

public class TimestampSaveChangesInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateEntities(DbContext? context)
    {
        if (context == null)
            return;

        var dateTime = DateTimeOffset.UtcNow;
        foreach (var entry in context.ChangeTracker.Entries<Timestamp>())
        {
            switch (entry.State)
            {
                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    entry.Entity.DeletedAt = dateTime;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = dateTime;
                    break;

                case EntityState.Added:
                    entry.Entity.CreatedAt = dateTime;
                    entry.Entity.UpdatedAt = dateTime;
                    break;
            }
        }
    }
}
