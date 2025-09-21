using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthConnect.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class ClientController(IClientService clientService) : ControllerBase
{
    public readonly IClientService _clientService = clientService;

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("all")]
    [Authorize(Roles = $"{AppRoles.Admin}")]
    public async Task<IActionResult> GetAllClientsAsync()
    {
        var clients = await _clientService.GetAllClientsAsync();
        return Ok(clients);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Doctor},{AppRoles.Patient}")]
    public async Task<IActionResult> GetClientByIdAsync(Guid id)
    {
        var client = await _clientService.GetClientByIdAsync(id);
        return Ok(client);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("user/{userId:guid}")]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Doctor},{AppRoles.Patient}")]
    public async Task<IActionResult> GetClientByUserIdAsync(Guid userId)
    {
        var client = await _clientService.GetClientByUserIdAsync(userId);
        return Ok(client);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("detail/{id:guid}")]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Patient}")]
    public async Task<IActionResult> GetClientDetailByIdAsync(Guid id)
    {
        var client = await _clientService.GetClientDetailByIdAsync(id);
        return Ok(client);
    }
}
