using HealthConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthConnect.Infrastructure.Data.Configurations;

public class AvailabilityConfiguration : IEntityTypeConfiguration<Availability>
{
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
    }
}
