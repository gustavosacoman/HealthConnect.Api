namespace HealthConnect.Application.Dtos.Auth;

/// <summary>
/// Data transfer object for user login requests.
/// </summary>
public record LoginRequestDto
{
    /// <summary>
    /// Gets the email address of the user attempting to log in.
    /// </summary>
    required public string Email { get; init; }

    /// <summary>
    /// Gets the password of the user attempting to log in.
    /// </summary>
    required public string Password { get; init; }
}
