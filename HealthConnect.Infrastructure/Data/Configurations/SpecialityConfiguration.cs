namespace HealthConnect.Infrastructure.Data.Configurations;

using HealthConnect.Domain.Models.Specialities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Entity Framework configuration for the <see cref="Speciality"/> entity.
/// </summary>
public class SpecialityConfiguration : IEntityTypeConfiguration<Speciality>
{
    /// <summary>
    /// Configures the <see cref="Speciality"/> entity type.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
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
