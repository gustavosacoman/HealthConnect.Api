namespace HealthConnect.Application.Dtos.Auth;

/// <summary>
/// Represents the response returned after a successful login, containing the authentication token.
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// Gets the authentication token issued to the user.
    /// </summary>
    required public string Token { get; init; }
}
