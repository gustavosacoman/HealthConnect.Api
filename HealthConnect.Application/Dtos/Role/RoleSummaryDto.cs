namespace HealthConnect.Application.Dtos.Role;

/// <summary>
/// Represents a summary of a role, including its unique identifier and name.
/// </summary>
public record RoleSummaryDto
{
    /// <summary>
    /// Gets the unique identifier of the role.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the name of the role.
    /// </summary>
    required public string Name { get; init; }
}
