namespace HealthConnect.Infrastructure.Data;

using HealthConnect.Domain.Interfaces;
using HealthConnect.Domain.Models;
using HealthConnect.Domain.Models.Roles;
using HealthConnect.Domain.Models.Specialities;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents the application's database context.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the users in the database.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Gets or sets the doctors in the database.
    /// </summary>
    public DbSet<Doctor> Doctors { get; set; }

    /// <summary>
    /// Gets or sets the clients in the database.
    /// </summary>
    public DbSet<Client> Clients { get; set; }

    /// <summary>
    /// Gets or sets the availabilities in the database.
    /// </summary>
    public DbSet<Availability> Availabilities { get; set; }

    /// <summary>
    /// Gets or sets the appointments in the database.
    /// </summary>
    public DbSet<Appointment> Appointments { get; set; }

    /// <summary>
    /// Gets or sets the specialities in the database.
    /// </summary>
    public DbSet<Speciality> Specialities { get; set; }

    /// <summary>
    /// Gets or sets the roles in the database.
    /// </summary>
    public DbSet<Role> Roles { get; set; }

    /// <summary>
    /// Gets or sets the user roles in the database.
    /// </summary>
    public DbSet<UserRole> UserRoles { get; set; }

    /// <summary>
    /// Gets or sets the doctor CRMs in the database.
    /// </summary>
    public DbSet<DoctorCRM> DoctorCRMs { get; set; }

    /// <summary>
    /// Gets or sets the doctor specialities in the database.
    /// </summary>
    public DbSet<DoctorSpeciality> DoctorSpecialities { get; set; }

    /// <summary>
    /// Gets or sets the doctor offices in the database.
    /// </summary>
    public DbSet<DoctorOffice> DoctorOffices { get; set; }

    /// <summary>
    /// Saves all changes made in this context to the database asynchronously and calls the updateAuditableFields.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>The number of state entries written to the database.</returns>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditableFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Configures the model for the context.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    /// <summary>
    /// Updates the auditable fields for entities tracked by the context.
    /// </summary>
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
