using HealthConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthConnect.Infrastructure.Data.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
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
