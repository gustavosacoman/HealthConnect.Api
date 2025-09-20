using HealthConnect.Domain.Interfaces;

namespace HealthConnect.Domain.Models.Roles;
public class Role : IAuditable, ISoftDeletable
{
    public Guid Id { get; set; }

    required public string Name { get; set; }

    public ICollection<UserRole> UserRoles { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

}
