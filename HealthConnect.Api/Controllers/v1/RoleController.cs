using HealthConnect.Application.Dtos.Role;
using HealthConnect.Application.Interfaces.ServicesInterface;
using Microsoft.AspNetCore.Mvc;

namespace HealthConnect.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class RoleController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;

    [HttpGet("{roleName}")]
    [ProducesResponseType(typeof(RoleSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoleByName(string roleName)
    {
        var role = await _roleService.GetRoleByNameAsync(roleName);
        return Ok(role);
    }

    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<RoleSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _roleService.GetAllRolesAsync();
        return Ok(roles);
    }

    [HttpGet("{roleId:guid}")]
    [ProducesResponseType(typeof(RoleSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoleById(Guid roleId)
    {
        var role = await _roleService.GetRoleByIdAsync(roleId);
        return Ok(role);
    }

    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<RoleSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRolesForUser(Guid userId)
    {
        var roles = await _roleService.GetRolesForUserAsync(userId);
        return Ok(roles);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRole(RoleRegistrationDto roleRegistration)
    {
        await _roleService.CreateRoleAsync(roleRegistration);
        return CreatedAtAction(nameof(GetRoleByName), new { roleName = roleRegistration.Name }, null);
    }
}
