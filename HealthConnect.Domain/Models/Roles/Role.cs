namespace HealthConnect.Domain.Models.Roles;

using HealthConnect.Domain.Interfaces;

/// <summary>
/// Represents a role within the HealthConnect system.
/// </summary>
public class Role : IAuditable, ISoftDeletable
{
    /// <summary>
    /// Gets or sets the unique identifier for the role.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the role.
    /// </summary>
    required public string Name { get; set; }

    /// <summary>
    /// Gets or sets the collection of user roles associated with this role.
    /// </summary>
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    /// <summary>
    /// Gets or sets the date and time when the role was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the role was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the role was deleted, if applicable.
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}
