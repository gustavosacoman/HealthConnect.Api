using HealthConnect.Application.Interfaces.ServicesInterface;
using Microsoft.AspNetCore.Mvc;

namespace HealthConnect.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class ClientController(IClientService clientService) : ControllerBase
{
    public readonly IClientService _clientService = clientService;

    [HttpGet("all")]
    public async Task<IActionResult> GetAllClientsAsync()
    {
        var clients = await _clientService.GetAllClientsAsync();
        return Ok(clients);
    }

    [HttpGet("{clientId:guid}")]
    public async Task<IActionResult> GetClientByIdAsync(Guid clientId)
    {
        var client = await _clientService.GetClientByIdAsync(clientId);
        return Ok(client);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetClientByUserIdAsync(Guid userId)
    {
        var client = await _clientService.GetClientByUserIdAsync(userId);
        return Ok(client);
    }

    [HttpGet("detail/{id:guid}")]
    public async Task<IActionResult> GetClientDetailByIdAsync(Guid id)
    {
        var client = await _clientService.GetClientDetailByIdAsync(id);
        return Ok(client);
    }
}
