namespace HealthConnect.Api.Controllers.v1;

using HealthConnect.Application.Dtos.Appointment;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller responsible for managing appointment operations in the HealthConnect system.
/// Provides endpoints for creating, retrieving, updating, and managing appointments between doctors and patients.
/// Handles appointment scheduling, status management, and appointment-related queries.
/// </summary>
/// <param name="appointmentService">The appointment service for handling appointment business logic.</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class AppointmentController(
    IAppointmentService appointmentService)
    : ControllerBase
{
    private readonly IAppointmentService _appointmentService = appointmentService;

    /// <summary>
    /// Creates a new appointment for the specified client with a doctor.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client for whom the appointment is being created.</param>
    /// <param name="appointment">The appointment registration details including availability ID and notes.</param>
    /// <returns>The created appointment details with a 201 Created status.</returns>
    /// <remarks>
    /// Sample request:
    ///     POST /api/v1/appointment/123e4567-e89b-12d3-a456-426614174000
    ///     {
    ///         "availabilityId": "789e0123-e89b-12d3-a456-426614174000",
    ///         "notes": "Regular check-up appointment for blood pressure monitoring"
    ///     }
    /// 
    /// **Business Rules:**
    /// - Client must exist in the system
    /// - Availability slot must exist and be available for booking
    /// - Availability slot must not already be booked
    /// - Appointment date must be in the future
    /// - Client cannot have overlapping appointments
    /// 
    /// **Validation Requirements:**
    /// - Client ID must be a valid GUID of an existing client
    /// - Availability ID must be a valid GUID of an available slot
    /// - Notes are optional but limited to 500 characters
    /// 
    /// **Use Cases:**
    /// - Patient booking an appointment through the system
    /// - Administrative staff scheduling appointments for patients
    /// - Integration with external booking systems
    /// - Automated appointment creation from waiting lists
    /// 
    /// **Access Control:**
    /// - Administrators: Can create appointments for any client
    /// - Patients: Can create appointments for themselves (client ID validation at service level)
    /// 
    /// **Created Response:**
    /// Returns the complete appointment details including doctor information, schedule details, and booking confirmation.
    /// </remarks>
    /// <response code="201">Appointment created successfully.</response>
    /// <response code="400">Invalid request data, availability slot unavailable, or business rule violation.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions to create appointments for this client.</response>
    /// <response code="404">Client or availability slot not found.</response>
    /// <response code="409">Conflict - availability slot already booked or client has overlapping appointment.</response>
    [ProducesResponseType(typeof(AppointmentDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
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
    /// Returns all appointments (past, present, and future) for the specified client including:
    /// - Complete appointment information (date, time, duration, status)
    /// - Doctor details (name and contact information)
    /// - Client information for verification
    /// - Appointment notes and status history
    /// 
    /// **Use Cases:**
    /// - Patient viewing their appointment history and upcoming appointments
    /// - Doctor reviewing patient's appointment history before consultation
    /// - Administrative staff managing patient schedules
    /// - Medical records integration and continuity of care
    /// - Appointment analytics and reporting
    /// 
    /// **Data Ordering:**
    /// Results are ordered chronologically with future appointments first, then past appointments in reverse chronological order.
    /// 
    /// **Access Control:**
    /// - Administrators: Can access any client's appointments
    /// - Doctors: Can access appointments for their patients
    /// - Patients: Can access their own appointments only (client ID validation at service level)
    /// 
    /// **Status Information:**
    /// Appointments include current status: Scheduled, Completed, CancelledByClient, CancelledByDoctor.
    /// </remarks>
    /// <response code="200">Appointments retrieved successfully.</response>
    /// <response code="400">Invalid client ID format.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions to access this client's appointments.</response>
    /// <response code="404">Client not found or no appointments found for the specified client.</response>
    [ProducesResponseType(typeof(IEnumerable<AppointmentDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    /// Returns all appointments (past, present, and future) for the specified doctor including:
    /// - Complete appointment scheduling information
    /// - Patient details and contact information
    /// - Appointment status and notes
    /// - Duration and availability information
    /// 
    /// **Use Cases:**
    /// - Doctor viewing their daily, weekly, or monthly schedule
    /// - Administrative staff managing doctor schedules and availability
    /// - Practice management and resource planning
    /// - Doctor performance analytics and reporting
    /// - Patient flow management and scheduling optimization
    /// 
    /// **Data Ordering:**
    /// Results are ordered chronologically starting with the earliest upcoming appointment, followed by future appointments, then past appointments in reverse order.
    /// 
    /// **Access Control:**
    /// - Administrators: Can access any doctor's appointment schedule
    /// - Doctors: Can access their own appointment schedule
    /// - Patients: May access limited appointment information for their own appointments with the doctor
    /// 
    /// **Schedule Management:**
    /// This endpoint is essential for calendar integration and schedule management systems used by medical practices.
    /// </remarks>
    /// <response code="200">Appointments retrieved successfully.</response>
    /// <response code="400">Invalid doctor ID format.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions to access this doctor's appointments.</response>
    /// <response code="404">Doctor not found or no appointments found for the specified doctor.</response>
    [ProducesResponseType(typeof(IEnumerable<AppointmentDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    ///     PATCH /api/v1/appointment?id=123e4567-e89b-12d3-a456-426614174000
    ///     {
    ///         "notes": "Patient requested to discuss medication side effects. Brought current medication list.",
    ///         "status": "Completed"
    ///     }
    /// 
    /// **Updateable Fields:**
    /// - **Notes**: Additional information, observations, or special instructions (max 1000 characters)
    /// - **Status**: Appointment status change (Scheduled, Completed, CancelledByClient, CancelledByDoctor)
    /// 
    /// **Business Rules:**
    /// - Only future appointments can be updated to "Scheduled" status
    /// - Past appointments can only be updated to "Completed" status
    /// - Cancelled appointments cannot be updated to other statuses
    /// - Notes can be updated at any time for record-keeping purposes
    /// 
    /// **Status Transitions:**
    /// - **Scheduled → Completed**: Normal appointment completion
    /// - **Scheduled → CancelledByDoctor**: Doctor-initiated cancellation
    /// - **Scheduled → CancelledByClient**: Patient-initiated cancellation
    /// - **Completed**: Final state, no further transitions allowed
    /// - **Cancelled**: Final state, no further transitions allowed
    /// 
    /// **Use Cases:**
    /// - Doctor updating appointment notes during or after consultation
    /// - Administrative staff marking appointments as completed
    /// - Handling appointment cancellations from either party
    /// - Adding clinical observations or special instructions
    /// - Appointment status management for billing and records
    /// 
    /// **Access Control:**
    /// Only administrators and doctors can update appointments to maintain data integrity and prevent unauthorized changes.
    /// </remarks>
    /// <response code="204">Appointment updated successfully.</response>
    /// <response code="400">Invalid request data, appointment ID, or invalid status transition.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions to update appointments.</response>
    /// <response code="404">Appointment not found.</response>
    /// <response code="409">Conflict - invalid status transition or appointment cannot be modified.</response>
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
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
    /// Returns comprehensive appointment details including:
    /// - **Basic Information**: Appointment ID, date, time, duration, and status
    /// - **Participant Details**: Complete doctor and client information
    /// - **Scheduling Information**: Availability slot reference and booking details
    /// - **Clinical Information**: Appointment notes and any special instructions
    /// - **Administrative Data**: Creation time, last update, and status history
    /// 
    /// **Use Cases:**
    /// - Viewing detailed appointment information for confirmation
    /// - Clinical workflow integration and appointment preparation
    /// - Administrative verification and appointment management
    /// - Patient portal displaying appointment details
    /// - Integration with calendar and scheduling systems
    /// - Medical records linking and continuity of care
    /// 
    /// **Access Control:**
    /// - Administrators: Can access details of any appointment
    /// - Doctors: Can access details of their appointments
    /// - Patients: Can access details of their own appointments (validation at service level)
    /// 
    /// **Data Completeness:**
    /// This endpoint provides the most complete view of an appointment, including all related entity information needed for clinical and administrative purposes.
    /// </remarks>
    /// <response code="200">Appointment details retrieved successfully.</response>
    /// <response code="400">Invalid appointment ID format.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions to access this appointment.</response>
    /// <response code="404">Appointment not found.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AppointmentDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Doctor},{AppRoles.Patient}")]
    public async Task<IActionResult> GetAppointmentById(Guid id)
    {
        var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
        return Ok(appointment);
    }

    /// <summary>
    /// Retrieves a summarized view of appointments for a specific doctor.
    /// </summary>
    /// <param name="doctorId">The unique identifier of the doctor whose appointment summary is being retrieved.</param>
    /// <returns>A collection of appointment summaries for the specified doctor.</returns>
    /// <remarks>
    /// Returns a lightweight summary of appointments optimized for dashboard and overview displays including:
    /// - **Essential Information**: Appointment ID, date, time, duration, and status
    /// - **Doctor Information**: Doctor ID and name for verification
    /// - **Scheduling Reference**: Availability ID for system linking
    /// - **Status Tracking**: Current appointment status for quick overview
    /// 
    /// **Differences from Detailed View:**
    /// - Excludes detailed client information (privacy-focused)
    /// - Excludes detailed notes (summary view only)
    /// - Optimized for faster loading and reduced data transfer
    /// - Suitable for calendar views and schedule overviews
    /// 
    /// **Use Cases:**
    /// - Doctor dashboard showing daily/weekly appointment overview
    /// - Schedule widgets and calendar integration
    /// - Quick appointment status checking
    /// - Mobile applications requiring lightweight data
    /// - Administrative dashboards and reporting summaries
    /// - Performance analytics and scheduling metrics
    /// 
    /// **Data Ordering:**
    /// Results are ordered chronologically starting with current and upcoming appointments.
    /// 
    /// **Performance Optimization:**
    /// This endpoint is optimized for frequent access and provides essential information with minimal overhead.
    /// </remarks>
    /// <response code="200">Appointment summary retrieved successfully.</response>
    /// <response code="400">Invalid doctor ID format.</response>
    /// <response code="404">Doctor not found or no appointments found for the specified doctor.</response>
    [HttpGet("summary/doctor/{doctorId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<AppointmentSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAppointmentSummaryByDoctorIdAsync(Guid doctorId)
    {
        var appointments = await _appointmentService.GetAppointmentSummaryByDoctorIdAsync(doctorId);
        return Ok(appointments);
    }
}