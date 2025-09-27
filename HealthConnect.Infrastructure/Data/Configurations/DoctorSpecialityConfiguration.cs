using HealthConnect.Domain.Models.Specialities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthConnect.Infrastructure.Data.Configurations;

public class DoctorSpecialityConfiguration : IEntityTypeConfiguration<DoctorSpeciality>
{
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
