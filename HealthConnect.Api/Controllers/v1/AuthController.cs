namespace HealthConnect.Api.Controllers.v1;

using HealthConnect.Application.Dtos.Auth;
using HealthConnect.Application.Interfaces.ServicesInterface;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller responsible for handling authentication operations in the HealthConnect system.
/// Provides endpoints for user authentication and token generation.
/// </summary>
/// <param name="authService">The authentication service for handling login operations.</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class AuthController(IAuthService authService)
    : ControllerBase
{
    private readonly IAuthService _authService = authService;

    /// <summary>
    /// Authenticates a user with email and password credentials and returns an authentication token.
    /// </summary>
    /// <param name="requestDto">The login request containing user email and password.</param>
    /// <returns>Authentication response containing the JWT token for authorized access.</returns>
    /// <remarks>
    /// Sample request:
    ///     POST /api/v1/auth/login
    ///     {
    ///         "email": "doctor@healthconnect.com",
    ///         "password": "SecurePassword123!"
    ///     }
    /// The returned token should be included in the Authorization header for subsequent requests:
    /// Authorization: Bearer {token}
    /// Token expiration and refresh mechanisms depend on the application's JWT configuration.
    /// </remarks>
    /// <response code="200">Authentication successful, returns JWT token.</response>
    /// <response code="400">Invalid request format or missing required fields.</response>
    /// <response code="401">Invalid credentials - email or password is incorrect.</response>
    /// <response code="404">User account not found.</response>
    /// <response code="423">User account is locked or disabled.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status423Locked)]
    public async Task<IActionResult> Authenticate([FromBody] LoginRequestDto requestDto)
    {
        var response = await _authService.LoginAsync(requestDto);
        return Ok(response);
    }
}