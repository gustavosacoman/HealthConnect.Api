
namespace HealthConnect.Application.Dtos;

/// <summary>
/// Data Transfer Object for updating user information.
/// </summary>
public record UserUpdatingDto
{
    /// <summary>
    /// Gets the user's name.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Gets the user's phone number.
    /// </summary>
    public string? Phone { get; init; }

    /// <summary>
    /// Gets the user's password.
    /// </summary>
    public string? Password { get; init; }
}
