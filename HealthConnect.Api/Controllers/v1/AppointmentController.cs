using HealthConnect.Application.Dtos.Appointment;
using HealthConnect.Application.Interfaces.ServicesInterface;
using Microsoft.AspNetCore.Mvc;

namespace HealthConnect.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class AppointmentController(IAppointmentService appointmentService) : ControllerBase
{
    private readonly IAppointmentService _appointmentService = appointmentService;

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("{clientId:guid}")]
    public async Task<IActionResult> CreateAppointmentAsync(
        [FromRoute] Guid clientId,
        [FromBody] AppointmentRegistrationDto appointment)
    {
        var createdAppointment =
            await _appointmentService
            .CreateAppointmentAsync(clientId, appointment);
        return CreatedAtAction(
            nameof(GetAppointmentById),
            new { id = createdAppointment.Id }, createdAppointment);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("by-client/{clientId:guid}")]
    public async Task<IActionResult> GetAppointmentsByClientIdAsync([FromRoute] Guid clientId)
    {
        var appointments = 
            await _appointmentService.GetAppointmentsByClientIdAsync(clientId);
        return Ok(appointments);
    }


    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("by-doctor/{doctorId:guid}")]
    public async Task<IActionResult> GetAppointmentsByDoctorIdAsync([FromRoute] Guid doctorId)
    {
        var appointments = 
            await _appointmentService.GetAppointmentsByDoctorIdAsync(doctorId);
        return Ok(appointments);
    }

    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAppointmentId(
        [FromQuery] Guid Id,
        [FromBody] AppointmentUpdatingDto appointment)
    {
        await _appointmentService.UpdateAppointmentId(Id, appointment);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AppointmentDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAppointmentById(Guid id)
    {
        var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
        return Ok(appointment);
    }

}
