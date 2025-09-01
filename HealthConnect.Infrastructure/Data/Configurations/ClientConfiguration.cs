using HealthConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthConnect.Infrastructure.Data.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");
        builder.HasIndex(c => c.Id).IsUnique();

        builder.HasIndex(c => c.UserId).IsUnique();
        builder.Property(c => c.UserId).IsRequired();

        builder.HasOne(c => c.User)
               .WithOne(u => u.Client)
               .HasForeignKey<Client>(c => c.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
