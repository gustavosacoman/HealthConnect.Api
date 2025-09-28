namespace HealthConnect.Api.Controllers.v1;

using HealthConnect.Application.Dtos.DoctorOffice;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller responsible for managing Doctor Offices in the HealthConnect system.
/// Provides endpoints for creating, retrieving, and managing doctor office locations,
/// including primary office designation and address management.
/// </summary>
/// <param name="doctorOfficeService">The doctor office service for handling office management business logic.</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class DoctorOfficeController(IDoctorOfficeService doctorOfficeService)
    : ControllerBase
{
    private readonly IDoctorOfficeService _doctorOfficeService = doctorOfficeService;

    /// <summary>
    /// Retrieves detailed information about a specific doctor office by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the doctor office to retrieve.</param>
    /// <returns>Detailed information about the specified doctor office including address and contact details.</returns>
    /// <remarks>
    /// Returns comprehensive office information including:
    /// - Office ID and associated doctor ID
    /// - Complete address (street, number, complement, state, zip code)
    /// - Contact information (phone numbers and secretary email)
    /// **Use Cases:**
    /// - Displaying office details for appointment booking
    /// - Office management and updates
    /// - Patient navigation and contact information
    /// - Administrative office verification
    /// This endpoint is publicly accessible to allow patients to view office information for appointment purposes.
    /// </remarks>
    /// <response code="200">Doctor office details retrieved successfully.</response>
    /// <response code="400">Invalid office ID format.</response>
    /// <response code="404">Doctor office not found.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DoctorOfficeSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOfficeById(Guid id)
    {
        var office = await _doctorOfficeService.GetOfficeByIdAsync(id);
        return Ok(office);
    }

    /// <summary>
    /// Retrieves all offices associated with a specific doctor.
    /// </summary>
    /// <param name="doctorId">The unique identifier of the doctor whose offices to retrieve.</param>
    /// <returns>A collection of all offices belonging to the specified doctor.</returns>
    /// <remarks>
    /// Returns a list of office locations for a specific doctor including:
    /// - Complete address information for each office
    /// - Contact details and secretary information
    /// - Office identifiers for booking and reference
    /// **Use Cases:**
    /// - Displaying available office locations during appointment booking
    /// - Doctor profile management showing all practice locations
    /// - Administrative oversight of doctor office networks
    /// - Patient convenience in choosing preferred office locations
    /// **Data Ordering:**
    /// Results may be ordered by primary office first, then by creation date or alphabetically by address.
    /// This endpoint is publicly accessible to help patients find convenient office locations.
    /// </remarks>
    /// <response code="200">Doctor offices retrieved successfully.</response>
    /// <response code="400">Invalid doctor ID format.</response>
    /// <response code="404">Doctor not found or no offices found for the specified doctor.</response>
    [HttpGet("all/by-doctor/{doctorId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<DoctorOfficeSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOfficesByDoctoId(Guid doctorId)
    {
        var offices = await _doctorOfficeService.GetOfficeByDoctorIdAsync(doctorId);
        return Ok(offices);
    }

    /// <summary>
    /// Retrieves all doctor offices in the system.
    /// </summary>
    /// <returns>A collection of all doctor office records in the system.</returns>
    /// <remarks>
    /// Returns a complete list of all registered doctor offices including:
    /// - Office addresses and contact information
    /// - Associated doctor identifiers
    /// - Primary office designations
    /// **Use Cases:**
    /// - Administrative oversight of all office locations
    /// - System-wide office management and reporting
    /// - Geographic analysis of medical service distribution
    /// - Integration with mapping and location services
    /// - Bulk operations on office data
    /// **Data Volume:**
    /// This endpoint may return large datasets. Consider implementing pagination for production use.
    /// **Access Control:**
    /// This endpoint is publicly accessible as office locations are typically public information
    /// necessary for patient care coordination and appointment booking.
    /// </remarks>
    /// <response code="200">All doctor offices retrieved successfully.</response>
    /// <response code="404">No doctor offices found in the system.</response>
    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<DoctorOfficeSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllDoctorOffices()
    {
        var offices = await _doctorOfficeService.GetAllDoctorOfficeAsync();
        return Ok(offices);
    }

    /// <summary>
    /// Retrieves the primary office for a specific doctor by doctor ID.
    /// </summary>
    /// <param name="doctorId">The unique identifier of the doctor whose primary office to retrieve.</param>
    /// <returns>The primary office information for the specified doctor.</returns>
    /// <remarks>
    /// Returns the designated primary office for the specified doctor, including:
    /// - Complete address and contact information
    /// - Primary office designation confirmation
    /// - Secretary contact details if available
    /// **Business Rules:**
    /// - Each doctor should have exactly one primary office
    /// - The primary office is used as the default location for appointments
    /// - Primary office information is prioritized in doctor profiles
    /// **Use Cases:**
    /// - Default office selection for appointment booking
    /// - Doctor profile display with main practice location
    /// - Administrative verification of primary practice locations
    /// - Patient communication and navigation assistance
    /// - Public directory services for medical office locations
    /// **Access Control:**
    /// This endpoint requires Doctor role authentication to ensure only authorized users can access
    /// primary office information, which may contain sensitive business details.
    /// </remarks>
    /// <response code="200">Primary office information retrieved successfully.</response>
    /// <response code="400">Invalid doctor ID format.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions to access primary office information.</response>
    /// <response code="404">No primary office found for the specified doctor.</response>
    [HttpGet("isprimary/office/{doctorId:guid}")]
    [ProducesResponseType(typeof(DoctorOfficeSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = AppRoles.Doctor)]
    public async Task<IActionResult> GetPrimaryOffice(Guid doctorId)
    {
        var office = await _doctorOfficeService.GetPrimaryOfficeByDoctorIdAsync(doctorId);
        return Ok(office);
    }

    /// <summary>
    /// Creates a new doctor office location.
    /// </summary>
    /// <param name="doctorOfficeRegistrationDto">The office registration details including address, contact information, and primary office designation.</param>
    /// <returns>Created response with location header pointing to the newly created office and the created office details.</returns>
    /// <remarks>
    /// Sample request:
    ///     POST /api/v1/doctoroffice
    ///     {
    ///         "doctorId": "123e4567-e89b-12d3-a456-426614174000",
    ///         "street": "Rua das Flores",
    ///         "number": 123,
    ///         "complement": "Sala 101",
    ///         "state": "SP",
    ///         "zipCode": "01234-567",
    ///         "phone": "(11) 1234-5678",
    ///         "secretaryPhone": "(11) 8765-4321",
    ///         "secretaryEmail": "secretary@doctoroffice.com",
    ///         "isPrimary": true
    ///     }
    /// **Business Rules:**
    /// - Each doctor can have multiple office locations
    /// - Only one office per doctor can be designated as primary
    /// - If creating the first office for a doctor, it should be set as primary
    /// - If setting an office as primary, any existing primary office is automatically updated
    /// **Validation Requirements:**
    /// - Doctor ID must be a valid GUID of an existing doctor
    /// - Street address and number are required fields
    /// - State must be a valid Brazilian state abbreviation
    /// - ZIP code must follow Brazilian postal code format
    /// - Phone numbers should follow Brazilian phone number format
    /// - Email addresses must be valid if provided
    /// **Use Cases:**
    /// - Doctors expanding practice to new locations
    /// - Initial office setup for new doctors
    /// - Updating practice locations due to relocations
    /// - Adding satellite offices or branch locations
    /// **Access Control:**
    /// Only administrators and doctors should be able to create office registrations.
    /// Doctors should typically only be able to create offices for themselves.
    /// </remarks>
    /// <response code="201">Doctor office created successfully. Returns the created office details.</response>
    /// <response code="400">Invalid request data, validation errors, or business rule violations.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions to create offices.</response>
    /// <response code="409">Conflict - primary office designation conflicts or duplicate office data.</response>
    [HttpPost]
    [ProducesResponseType(typeof(DoctorOfficeSummaryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Doctor}")]
    public async Task<IActionResult> CreateDoctorOffice([FromBody] DoctorOfficeRegistrationDto doctorOfficeRegistrationDto)
    {
        var createdOffice = await _doctorOfficeService.CreateDoctorOfficeAsync(doctorOfficeRegistrationDto);
        return CreatedAtAction(
            nameof(GetOfficeById),
            new { id = createdOffice.Id },
            createdOffice);
    }
}