namespace HealthConnect.Domain.Interfaces;

/// <summary>
/// Represents an entity that supports soft deletion by tracking the deletion timestamp.
/// </summary>
public interface ISoftDeletable
{
    /// <summary>
    /// Gets or sets the date and time when the entity was deleted. Null if not deleted.
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}
