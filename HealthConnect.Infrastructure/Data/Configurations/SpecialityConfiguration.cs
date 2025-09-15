namespace HealthConnect.Infrastructure.Data.Configurations;

using HealthConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SpecialityConfiguration : IEntityTypeConfiguration<Speciality>
{
    public void Configure(EntityTypeBuilder<Speciality> builder)
    {
        builder.ToTable("Specialities");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(s => s.Name).IsUnique();
    }
}
