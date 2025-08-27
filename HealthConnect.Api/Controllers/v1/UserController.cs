namespace HealthConnect.Api.Controllers.v1;

using HealthConnect.Application.Dtos.Users;
using HealthConnect.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller for managing user-related operations.
/// </summary>
/// <remarks>
/// Provides endpoints to retrieve, create, update, and delete user information.
/// </remarks>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Authorize]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    /// <summary>
    /// Gets a user by their unique identifier.
    /// </summary>
    /// <param name="id">The user's unique identifier.</param>
    /// <returns>The user summary.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetUserById(id);
        return Ok(user);
    }

    /// <summary>
    /// Gets a user by their email address.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <returns>The user summary.</returns>
    [HttpGet("by-email/{email}")]
    [ProducesResponseType(typeof(UserSummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        var user = await _userService.GetUserByEmail(email);
        return Ok(user);
    }

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <returns>A list of user summaries.</returns>
    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<UserSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsers();
        return Ok(users);
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="data">The user registration data.</param>
    /// <returns>The created user summary.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(UserSummaryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] UserRegistrationDto data)
    {
        var user = await _userService.CreateUser(data);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="id">The user's unique identifier.</param>
    /// <param name="data">The user update data.</param>
    /// <returns>The updated user summary.</returns>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(UserSummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserUpdatingDto data)
    {
        var user = await _userService.UpdateUser(id, data);
        return Ok(user);
    }

    /// <summary>
    /// Deletes a user by their email address.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <returns>No content.</returns>
    [HttpDelete("{email}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(string email)
    {
        await _userService.DeleteUser(email);
        return NoContent();
    }
}
