using HealthConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Infrastructure.Data.Configurations;

public class DoctorCRMConfiguration : IEntityTypeConfiguration<DoctorCRM>
{
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
