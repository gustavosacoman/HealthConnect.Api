namespace HealthConnect.Domain.Models.Roles;

using HealthConnect.Domain.Interfaces;

/// <summary>
/// Represents the association between a user and a role, including audit and soft delete information.
/// </summary>
public class UserRole : IAuditable, ISoftDeletable
{
    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the user associated with this role.
    /// </summary>
    required public User User { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the role.
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// Gets or sets the role associated with the user.
    /// </summary>
    required public Role Role { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the association was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the association was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the association was deleted, if applicable.
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}
