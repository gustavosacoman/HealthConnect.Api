namespace HealthConnect.Application.Interfaces.ServicesInterface;

using HealthConnect.Application.Dtos.Auth;

/// <summary>
/// Provides authentication services for user login.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticates a user with the provided login request and returns a login response containing a token.
    /// </summary>
    /// <param name="request">The login request containing user credentials.</param>
    /// <returns>A <see cref="LoginResponseDto"/> containing the authentication token.</returns>
    public Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
}
