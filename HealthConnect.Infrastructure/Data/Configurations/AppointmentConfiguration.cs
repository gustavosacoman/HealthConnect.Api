namespace HealthConnect.Infrastructure.Data.Configurations;

using HealthConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Provides configuration for the <see cref="Appointment"/> entity type.
/// </summary>
public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    /// <summary>
    /// Configures the <see cref="Appointment"/> entity type.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Apointments");

        builder.Property(a => a.ClientId)
            .IsRequired();

        builder.Property(a => a.DoctorId)
            .IsRequired();

        builder.Property(a => a.AvailabilityId)
            .IsRequired();

        builder.Property(a => a.AppointmentStatus)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(a => a.AppointmentDateTime)
            .IsRequired();

        builder.Property(a => a.Notes)
            .HasMaxLength(1000);
    }
}
