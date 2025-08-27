namespace HealthConnect.Api.Controllers.v1;

using HealthConnect.Application.Dtos.Auth;
using HealthConnect.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class AuthController(IAuthService authService) : ControllerBase
{
    public readonly IAuthService _authService = authService;

    [HttpPost("login")]
    public async Task<IActionResult> Authenticate([FromBody] LoginRequestDto requestDto)
    {
        var response = await _authService.LoginAsync(requestDto);
        return Ok(response);
    }

}
