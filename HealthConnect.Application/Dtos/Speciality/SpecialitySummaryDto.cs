namespace HealthConnect.Application.Dtos.Speciality;

/// <summary>
/// Represents a summary of a medical speciality.
/// </summary>
public record SpecialitySummaryDto
{
    /// <summary>
    /// Gets the unique identifier of the speciality.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the name of the speciality.
    /// </summary>
    public string Name { get; init; }
}
