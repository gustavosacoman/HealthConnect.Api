using HealthConnect.Domain.Interfaces;
using HealthConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthConnect.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditableFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditableFields()
    {
        var timestamp = DateTime.UtcNow;

        var entries = ChangeTracker.Entries<IAuditable>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(p => p.CreatedAt).CurrentValue = timestamp;
                entry.Property(p => p.UpdatedAt).CurrentValue = timestamp;

            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property(p => p.UpdatedAt).CurrentValue = timestamp;
            }
        }
    }
}
