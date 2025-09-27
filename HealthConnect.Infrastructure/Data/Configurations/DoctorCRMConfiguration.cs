namespace HealthConnect.Infrastructure.Data.Configurations;

using HealthConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Entity Framework configuration for the <see cref="DoctorCRM"/> entity.
/// </summary>
public class DoctorCRMConfiguration : IEntityTypeConfiguration<DoctorCRM>
{
    /// <summary>
    /// Configures the <see cref="DoctorCRM"/> entity type.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<DoctorCRM> builder)
    {
        builder.ToTable("DoctorCRMs");

        builder.HasKey(e => e.Id);

        builder.Property(c => c.State)
            .IsRequired()
            .HasMaxLength(2);

        builder.Property(c => c.CRMNumber)
            .IsRequired()
            .HasMaxLength(6);

        builder.HasIndex(c => new { c.CRMNumber, c.State })
            .IsUnique();

        builder.Property(c => c.DoctorId)
            .IsRequired();

        builder.HasOne(c => c.Doctor)
            .WithMany(d => d.DoctorCRMs)
            .HasForeignKey(c => c.DoctorId);
    }
}
