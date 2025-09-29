namespace HealthConnect.Infrastructure.Data.Configurations;

using HealthConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Entity Framework configuration for the <see cref="DoctorOffice"/> entity.
/// </summary>
public class DoctorOfficeConfiguration : IEntityTypeConfiguration<DoctorOffice>
{
    /// <summary>
    /// Configures the <see cref="DoctorOffice"/> entity type.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<DoctorOffice> builder)
    {
        builder.ToTable("DoctorOffices");

        builder.HasKey(of => of.Id);

        builder.Property(of => of.Street)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(of => of.Number)
            .IsRequired();

        builder.Property(of => of.Complement)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(of => of.State)
            .IsRequired()
            .HasMaxLength(2);

        builder.Property(of => of.ZipCode)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(of => of.Phone)
            .HasMaxLength(15);

        builder.Property(of => of.SecretaryPhone)
            .HasMaxLength(15);

        builder.Property(of => of.SecretaryEmail)
            .HasMaxLength(100);

        builder.HasIndex(of => new { of.DoctorId, of.IsPrimary })
            .IsUnique()
            .HasFilter("\"IsPrimary\" = true");

        builder.Property(of => of.City)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(of => of.Neighborhood)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne(of => of.Doctor)
            .WithMany(d => d.DoctorOffices)
            .HasForeignKey(of => of.DoctorId);
    }
}
