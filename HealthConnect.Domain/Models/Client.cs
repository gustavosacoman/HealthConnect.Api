namespace HealthConnect.Domain.Models;

using HealthConnect.Domain.Interfaces;

/// <summary>
/// Represents a client entity in the HealthConnect domain.
/// </summary>
public class Client : IAuditable, ISoftDeletable
{
    /// <summary>
    /// Gets or sets the unique identifier for the client.
    /// </summary>
    required public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated user.
    /// </summary>
    required public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the associated user entity.
    /// </summary>
    required public virtual User User { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the client was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the client was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the client was deleted, if applicable.
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}
