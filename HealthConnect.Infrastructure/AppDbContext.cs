using HealthConnect.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthConnect.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditableFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditableFields()
    {
        var entries = ChangeTracker.Entries<IAuditable>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(p => p.CreateAt).CurrentValue = DateTime.UtcNow;
                entry.Property(p => p.UpdateAt).CurrentValue = DateTime.UtcNow;

            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property(p => p.UpdateAt).CurrentValue = DateTime.UtcNow;
            }
        }
    }
}
