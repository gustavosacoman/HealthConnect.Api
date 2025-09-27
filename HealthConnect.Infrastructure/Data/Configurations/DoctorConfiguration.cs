namespace HealthConnect.Infrastructure.Data.Configurations;

using HealthConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Entity Framework configuration for the <see cref="Doctor"/> entity.
/// </summary>
public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    /// <summary>
    /// Configures the <see cref="Doctor"/> entity type.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("Doctors");

        builder.HasKey(d => d.Id);

        builder.HasIndex(d => d.RQE)
            .IsUnique();
        builder.Property(d => d.RQE)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(d => d.UserId)
            .IsUnique();

        builder.Property(d => d.Biography)
            .HasMaxLength(3500);

        // builder.Property(d => d.ProfilePicture)
        // .HasMaxLength(400);
        builder.HasQueryFilter(d => d.DeletedAt == null);

        builder.HasOne(d => d.User)
            .WithOne(u => u.Doctor)
            .HasForeignKey<Doctor>(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
