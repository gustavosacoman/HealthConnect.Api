namespace HealthConnect.Infrastructure.Data.Configurations;

using HealthConnect.Domain.Models.Specialities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Entity Framework configuration for the <see cref="DoctorSpeciality"/> entity.
/// </summary>
public class DoctorSpecialityConfiguration : IEntityTypeConfiguration<DoctorSpeciality>
{
    /// <summary>
    /// Configures the <see cref="DoctorSpeciality"/> entity type.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<DoctorSpeciality> builder)
    {
        builder.ToTable("DoctorSpecialities");

        builder.Property(ds => ds.RqeNumber)
               .IsRequired()
               .HasMaxLength(9);

        builder.HasIndex(ds => ds.RqeNumber)
            .IsUnique();

        builder.HasKey(ds => new { ds.DoctorId, ds.SpecialityId });

        builder.HasOne(ds => ds.Speciality)
               .WithMany(s => s.DoctorSpecialities)
               .HasForeignKey(ds => ds.SpecialityId);

        builder.HasOne(ds => ds.Doctor)
            .WithMany(d => d.DoctorSpecialities)
            .HasForeignKey(ds => ds.DoctorId);
    }
}
