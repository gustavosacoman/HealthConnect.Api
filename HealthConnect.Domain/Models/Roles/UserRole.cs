namespace HealthConnect.Domain.Models.Roles;

using HealthConnect.Domain.Interfaces;

public class UserRole : IAuditable, ISoftDeletable
{
    public Guid UserId { get; set; }

    public User User { get; set; }

    public Guid RoleId { get; set; }

    public Role Role { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
