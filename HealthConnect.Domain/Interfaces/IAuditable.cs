namespace HealthConnect.Domain.Interfaces;

/// <summary>
/// Represents an auditable entity with creation and update timestamps.
/// </summary>
public interface IAuditable
{
    /// <summary>
    /// Gets or sets the date and time when the entity was created.
    /// </summary>
    DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was last updated.
    /// </summary>
    DateTime UpdatedAt { get; set; }
}
