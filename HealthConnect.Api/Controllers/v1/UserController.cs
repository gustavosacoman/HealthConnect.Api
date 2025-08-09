using HealthConnect.Application.Dtos;
using HealthConnect.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HealthConnect.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {

        var user = await _userService.GetUserById(id);
        return Ok(user);
    }

    [HttpGet("by-email/{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {

        var user = await _userService.GetUserByEmail(email);
        return Ok(user);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsers();
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserRegistrationDto data)
    {

        var user = await _userService.CreateUser(data);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserUpdatingDto data)
    {
        var user = await _userService.UpdateUser(id, data);
        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        await _userService.DeleteUser(id);
        return NoContent();
    }

}
