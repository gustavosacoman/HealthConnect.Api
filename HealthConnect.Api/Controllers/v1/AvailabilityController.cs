namespace HealthConnect.Api.Controllers.v1;

using HealthConnect.Application.Dtos.Availability;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class AvailabilityController(IAvailabilityService availabilityService) : ControllerBase
{
    private readonly IAvailabilityService _availabilityService = availabilityService;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Doctor}")]
    public async Task<IActionResult> CreateAvailability(AvailabilityRegistrationDto availability)
    {
        var createdAvailability = await _availabilityService.CreateAvailabilityAsync(availability);

        return CreatedAtAction(
            nameof(GetAvailabilityById),
            new { availabilityId = createdAvailability.Id },
            createdAvailability);
    }

    [HttpGet("{availabilityId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAvailabilityById(Guid availabilityId)
    {
        var availability = await _availabilityService.GetAvailabilityByIdAsync(availabilityId);
        return Ok(availability);
    }

    [HttpGet("by-doctor/{doctorId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAvailabilitiesPerDoctor(Guid doctorId)
    {
        var availabilities = await _availabilityService.GetAllAvailabilitiesPerDoctorAsync(doctorId);
        return Ok(availabilities);
    }

    [HttpDelete]
    [Route("{availabilityId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Doctor}")]
    public async Task<IActionResult> DeleteAvailability(Guid availabilityId)
    {
        await _availabilityService.DeleteAvailabilityAsync(availabilityId);
        return NoContent();
    }
}
