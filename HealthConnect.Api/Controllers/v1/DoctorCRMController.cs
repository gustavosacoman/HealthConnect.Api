using HealthConnect.Application.Dtos.DoctorCRM;
using HealthConnect.Application.Interfaces.ServicesInterface;
using Microsoft.AspNetCore.Mvc;

namespace HealthConnect.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class DoctorCRMController(IDoctorCRMService doctorCRMService) : ControllerBase
{
    private readonly IDoctorCRMService _doctorCRMService = doctorCRMService;

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCRMByIdAsync(Guid id)
    {
        var crm = await _doctorCRMService.GetCRMByIdAsync(id);
        return Ok(crm);
    }

    [HttpGet("by-code")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCRMByCodeAndState([FromQuery] string crmNumber, [FromQuery] string state)
    {
        var crm = await _doctorCRMService.GetCRMByCodeAndState(crmNumber, state);
        return Ok(crm);
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllCRMAsync()
    {
        var crms = await _doctorCRMService.GetAllCRMAsync();
        return Ok(crms);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCRMAsync([FromBody] DoctorCRMRegistrationDto doctorCRMDto)
    {
        await _doctorCRMService.CreateCRMAsync(doctorCRMDto);
        return CreatedAtAction(nameof(GetCRMByIdAsync), new { id = doctorCRMDto.DoctorId }, doctorCRMDto);
    }

}
