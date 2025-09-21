namespace HealthConnect.Api.Controllers.v1;

using HealthConnect.Application.Dtos.Speciality;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class SpecialityController(ISpecialityService specialityService) : ControllerBase
{
    private readonly ISpecialityService _specialityService = specialityService;

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSpecialityById(Guid id)
    {
        var speciality = await _specialityService.GetSpecialityById(id);
        return Ok(speciality);
    }

    [HttpGet("by-name/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSpecialityByName(string name)
    {
        var speciality = await _specialityService.GetSpecialityByName(name);
        return Ok(speciality);
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllSpecialities()
    {
        var specialities = await _specialityService.GetAllSpecialities();
        return Ok(specialities);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = $"{AppRoles.Admin}")]
    public async Task<IActionResult> CreateSpeciality([FromBody] SpecialityRegistrationDto specialityDto)
    {
        var createdSpeciality = await _specialityService.CreateSpeciality(specialityDto);
        return CreatedAtAction(nameof(GetSpecialityById), new { id = createdSpeciality.Id }, createdSpeciality);
    }
}
