namespace HealthConnect.Api.Controllers.v1;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Application.Dtos.Availability;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Authorize]
public class AvailabilityController(IAvailabilityService availabilityService) : ControllerBase
{
    private readonly IAvailabilityService _availabilityService = availabilityService;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
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
    public async Task<IActionResult> GetAvailabilityById(Guid availabilityId)
    {
        var availability = await _availabilityService.GetAvailabilityByIdAsync(availabilityId);
        return Ok(availability);
    }

    [HttpGet("by-doctor/{doctorId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAvailabilitiesPerDoctor(Guid doctorId)
    {
        var availabilities = await _availabilityService.GetAllAvailabilitiesPerDoctorAsync(doctorId);
        return Ok(availabilities);
    }

    [HttpDelete]
    [Route("{availabilityId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAvailability(Guid availabilityId)
    {
        await _availabilityService.DeleteAvailabilityAsync(availabilityId);
        return NoContent();
    }
}
