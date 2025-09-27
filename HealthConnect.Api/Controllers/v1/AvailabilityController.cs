namespace HealthConnect.Api.Controllers.v1;

using HealthConnect.Application.Dtos.Availability;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller responsible for managing doctor availability slots in the HealthConnect system.
/// Provides endpoints for creating, retrieving, and deleting availability time slots that can be booked for appointments.
/// </summary>
/// <param name="availabilityService">The availability service for handling availability slot business logic.</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class AvailabilityController(IAvailabilityService availabilityService)
    : ControllerBase
{
    private readonly IAvailabilityService _availabilityService = availabilityService;

    /// <summary>
    /// Creates a new availability slot for a doctor.
    /// </summary>
    /// <param name="availability">The availability slot details including doctor ID, date/time, and duration.</param>
    /// <returns>The created availability slot with detailed information.</returns>
    /// <remarks>
    /// Sample request:
    ///     POST /api/v1/availability
    ///     {
    ///         "doctorId": "123e4567-e89b-12d3-a456-426614174000",
    ///         "slotDateTime": "2024-12-25T14:00:00Z",
    ///         "durationMinutes": 30
    ///     }
    /// The slot date/time must be in the future and the duration must be a positive integer.
    /// Only administrators and doctors can create availability slots.
    /// Doctors can typically only create slots for themselves (business rule enforced at service level).
    /// </remarks>
    /// <response code="201">Availability slot created successfully.</response>
    /// <response code="400">Invalid request data, slot conflicts, or business rule violation.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions.</response>
    /// <response code="404">Doctor not found.</response>
    [HttpPost]
    [ProducesResponseType(typeof(AvailabilitySummaryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Doctor}")]
    public async Task<IActionResult> CreateAvailability([FromBody] AvailabilityRegistrationDto availability)
    {
        var createdAvailability = await _availabilityService.CreateAvailabilityAsync(availability);

        return CreatedAtAction(
            nameof(GetAvailabilityById),
            new { availabilityId = createdAvailability.Id },
            createdAvailability);
    }

    /// <summary>
    /// Retrieves detailed information about a specific availability slot.
    /// </summary>
    /// <param name="availabilityId">The unique identifier of the availability slot to retrieve.</param>
    /// <returns>Detailed information about the specified availability slot including doctor details.</returns>
    /// <remarks>
    /// Returns comprehensive availability slot information including:
    /// - Doctor information (name, specialty, specialties)
    /// - Slot date and time
    /// - Duration in minutes
    /// - Availability status
    /// This endpoint is publicly accessible to allow patients to view available slots for booking.
    /// </remarks>
    /// <response code="200">Availability slot details retrieved successfully.</response>
    /// <response code="404">Availability slot not found.</response>
    /// <response code="400">Invalid availability slot ID format.</response>
    [HttpGet("{availabilityId:guid}")]
    [ProducesResponseType(typeof(AvailabilitySummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAvailabilityById(Guid availabilityId)
    {
        var availability = await _availabilityService.GetAvailabilityByIdAsync(availabilityId);
        return Ok(availability);
    }

    /// <summary>
    /// Retrieves all availability slots for a specific doctor.
    /// </summary>
    /// <param name="doctorId">The unique identifier of the doctor whose availability slots are being retrieved.</param>
    /// <returns>A collection of all availability slots for the specified doctor.</returns>
    /// <remarks>
    /// Returns all availability slots (past, present, and future) for the specified doctor.
    /// This includes both available and booked slots.
    /// Useful for:
    /// - Doctors to view their own schedules
    /// - Patients to see available appointment times
    /// - Administrators to manage doctor schedules
    /// This endpoint is publicly accessible to allow patients to view doctor availability.
    /// </remarks>
    /// <response code="200">Doctor availability slots retrieved successfully.</response>
    /// <response code="404">Doctor not found.</response>
    /// <response code="400">Invalid doctor ID format.</response>
    [HttpGet("by-doctor/{doctorId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<AvailabilitySummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllAvailabilitiesPerDoctor(Guid doctorId)
    {
        var availabilities = await _availabilityService.GetAllAvailabilitiesPerDoctorAsync(doctorId);
        return Ok(availabilities);
    }

    /// <summary>
    /// Deletes a specific availability slot.
    /// </summary>
    /// <param name="availabilityId">The unique identifier of the availability slot to delete.</param>
    /// <returns>No content response indicating successful deletion.</returns>
    /// <remarks>
    /// Permanently removes an availability slot from the system.
    /// **Important considerations:**
    /// - Cannot delete slots that have existing appointments booked
    /// - Only administrators and doctors can delete availability slots
    /// - Doctors can typically only delete their own slots (business rule enforced at service level)
    /// - Consider the impact on patient scheduling before deletion
    /// **Alternative approaches:**
    /// - Consider implementing slot deactivation instead of deletion for audit purposes
    /// - Notify affected patients if appointments exist (handled at service level).
    /// </remarks>
    /// <response code="204">Availability slot deleted successfully.</response>
    /// <response code="404">Availability slot not found.</response>
    /// <response code="400">Cannot delete slot with existing appointments or other business rule violation.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions.</response>
    [HttpDelete]
    [Route("{availabilityId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Doctor}")]
    public async Task<IActionResult> DeleteAvailability(Guid availabilityId)
    {
        await _availabilityService.DeleteAvailabilityAsync(availabilityId);
        return NoContent();
    }
}