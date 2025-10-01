namespace HealthConnect.Api.Controllers.v1;

using HealthConnect.Application.Dtos.Appointment;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller responsible for managing appointment operations in the HealthConnect system.
/// Provides endpoints for creating, retrieving, and updating appointments.
/// </summary>
/// <param name="appointmentService">The appointment service for handling business logic.</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class AppointmentController(
    IAppointmentService appointmentService)
    : ControllerBase
{
    private readonly IAppointmentService _appointmentService = appointmentService;

    /// <summary>
    /// Creates a new appointment for the specified client.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client for whom the appointment is being created.</param>
    /// <param name="appointment">The appointment registration details including availability ID and notes.</param>
    /// <returns>The created appointment details with a 201 Created status.</returns>
    /// <remarks>
    /// Sample request:
    ///     POST /api/v1/appointment/123e4567-e89b-12d3-a456-426614174000
    ///     {
    ///         "availabilityId": "789e0123-e89b-12d3-a456-426614174000",
    ///         "notes": "Regular check-up appointment"
    ///     }
    /// Only administrators and doctors can create appointments.
    /// </remarks>
    /// <response code="201">Appointment created successfully.</response>
    /// <response code="400">Invalid request data or business rule violation.</response>
    /// <response code="404">Client or availability slot not found.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions.</response>
    [ProducesResponseType(typeof(AppointmentDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpPost("{clientId:guid}")]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Patient}")]
    public async Task<IActionResult> CreateAppointmentAsync(
        [FromRoute] Guid clientId,
        [FromBody] AppointmentRegistrationDto appointment)
    {
        var createdAppointment =
            await _appointmentService
            .CreateAppointmentAsync(clientId, appointment);
        return CreatedAtAction(
    nameof(GetAppointmentById),
    new { id = createdAppointment.Id },
    createdAppointment);
    }

    /// <summary>
    /// Retrieves all appointments for a specific client.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client whose appointments are being retrieved.</param>
    /// <returns>A collection of appointment details for the specified client.</returns>
    /// <remarks>
    /// Returns all appointments (past, present, and future) for the specified client.
    /// Accessible by administrators, doctors, and the patient themselves.
    /// </remarks>
    /// <response code="200">Appointments retrieved successfully.</response>
    /// <response code="400">Invalid client ID format.</response>
    /// <response code="404">Client not found.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions.</response>
    [ProducesResponseType(typeof(IEnumerable<AppointmentDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpGet("by-client/{clientId:guid}")]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Doctor},{AppRoles.Patient}")]
    public async Task<IActionResult> GetAppointmentsByClientIdAsync([FromRoute] Guid clientId)
    {
        var appointments =
            await _appointmentService.GetAppointmentsByClientIdAsync(clientId);
        return Ok(appointments);
    }

    /// <summary>
    /// Retrieves all appointments for a specific doctor.
    /// </summary>
    /// <param name="doctorId">The unique identifier of the doctor whose appointments are being retrieved.</param>
    /// <returns>A collection of appointment details for the specified doctor.</returns>
    /// <remarks>
    /// Returns all appointments (past, present, and future) for the specified doctor.
    /// Useful for doctors to view their schedule and for administrators to manage appointments.
    /// </remarks>
    /// <response code="200">Appointments retrieved successfully.</response>
    /// <response code="400">Invalid doctor ID format.</response>
    /// <response code="404">Doctor not found.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions.</response>
    [ProducesResponseType(typeof(IEnumerable<AppointmentDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpGet("by-doctor/{doctorId:guid}")]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Doctor},{AppRoles.Patient}")]
    public async Task<IActionResult> GetAppointmentsByDoctorIdAsync([FromRoute] Guid doctorId)
    {
        var appointments =
            await _appointmentService.GetAppointmentsByDoctorIdAsync(doctorId);
        return Ok(appointments);
    }

    /// <summary>
    /// Updates an existing appointment with new information.
    /// </summary>
    /// <param name="id">The unique identifier of the appointment to update.</param>
    /// <param name="appointment">The appointment update details including notes and status.</param>
    /// <returns>No content response indicating successful update.</returns>
    /// <remarks>
    /// Sample request:
    ///     PATCH /api/v1/appointment?Id=123e4567-e89b-12d3-a456-426614174000
    ///     {
    ///         "notes": "Updated appointment notes",
    ///         "status": "Completed"
    ///     }
    /// Only administrators and doctors can update appointments.
    /// Valid status values: Scheduled, Completed, CancelledByClient, CancelledByDoctor.
    /// </remarks>
    /// <response code="204">Appointment updated successfully.</response>
    /// <response code="400">Invalid request data or appointment ID.</response>
    /// <response code="404">Appointment not found.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions.</response>
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Doctor}")]
    public async Task<IActionResult> UpdateAppointmentId(
        [FromQuery] Guid id,
        [FromBody] AppointmentUpdatingDto appointment)
    {
        await _appointmentService.UpdateAppointmentId(id, appointment);
        return NoContent();
    }

    /// <summary>
    /// Retrieves detailed information about a specific appointment.
    /// </summary>
    /// <param name="id">The unique identifier of the appointment to retrieve.</param>
    /// <returns>Detailed information about the specified appointment.</returns>
    /// <remarks>
    /// Returns comprehensive appointment details including client information, doctor information,
    /// appointment date and time, duration, status, and notes.
    /// </remarks>
    /// <response code="200">Appointment details retrieved successfully.</response>
    /// <response code="404">Appointment not found.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AppointmentDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Doctor},{AppRoles.Patient}")]
    public async Task<IActionResult> GetAppointmentById(Guid id)
    {
        var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
        return Ok(appointment);
    }
}