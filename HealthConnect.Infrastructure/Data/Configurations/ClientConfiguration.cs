namespace HealthConnect.Infrastructure.Data.Configurations;

using HealthConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Entity Framework configuration for the <see cref="Client"/> entity.
/// </summary>
public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    /// <summary>
    /// Configures the <see cref="Client"/> entity type.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
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
