namespace HealthConnect.Infrastructure.Data.Configurations;

using HealthConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Entity Framework configuration for the <see cref="Availability"/> entity.
/// </summary>
public class AvailabilityConfiguration : IEntityTypeConfiguration<Availability>
{
    /// <summary>
    /// Configures the <see cref="Availability"/> entity type.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Availability> builder)
    {
        builder.ToTable("Avaiabilities");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.DoctorId)
            .IsRequired();

        builder.Property(a => a.SlotDateTime)
               .IsRequired();

        builder.Property(a => a.DurationMinutes)
            .IsRequired();

        builder.Property(a => a.IsBooked)
            .IsRequired();

        builder.HasOne(a => a.Doctor)
            .WithMany(d => d.Availabilities)
            .HasForeignKey(a => a.DoctorId);

        builder.HasOne(a => a.DoctorOffice)
            .WithMany(of =>  of.Availabilities)
            .HasForeignKey(of => of.DoctorOfficeId);
    }
}
