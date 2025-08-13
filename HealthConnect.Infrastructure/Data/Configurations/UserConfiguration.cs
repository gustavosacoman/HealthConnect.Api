using HealthConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.CPF)
            .IsRequired()
            .HasMaxLength(11);

        builder.HasIndex(u => u.CPF)
            .IsUnique();

        builder.Property(u => u.Phone)
            .HasMaxLength(15);

        builder.Property(u => u.HashedPassword)
            .IsRequired();

        builder.Property(u => u.Salt)
            .IsRequired();

        builder.Property(u => u.BirthDate)
            .IsRequired();

        builder.HasQueryFilter(u => u.DeletedAt == null);
    }
}
