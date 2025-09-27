namespace HealthConnect.Application.Dtos.Role;

/// <summary>
/// Data Transfer Object for registering a new role.
/// </summary>
public record RoleRegistrationDto
{
    /// <summary>
    /// Gets the name of the role.
    /// </summary>
    required public string Name { get; init; }
}
