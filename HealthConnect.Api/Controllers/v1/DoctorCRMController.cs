namespace HealthConnect.Api.Controllers.v1;

using HealthConnect.Application.Dtos.DoctorCRM;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller responsible for managing Doctor CRM (Conselho Regional de Medicina) registrations in the HealthConnect system.
/// Provides endpoints for creating, retrieving, and managing CRM records that validate medical professional licensing.
/// </summary>
/// <param name="doctorCRMService">The doctor CRM service for handling medical license registration business logic.</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class DoctorCRMController(IDoctorCRMService doctorCRMService)
    : ControllerBase
{
    private readonly IDoctorCRMService _doctorCRMService = doctorCRMService;

    /// <summary>
    /// Retrieves detailed information about a specific CRM registration by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the CRM registration to retrieve.</param>
    /// <returns>Detailed information about the specified CRM registration including doctor details.</returns>
    /// <remarks>
    /// Returns comprehensive CRM registration information including:
    /// - CRM registration ID and doctor ID
    /// - Doctor name and CRM number
    /// - State where the CRM is registered
    /// This endpoint is publicly accessible to allow verification of medical professional credentials.
    /// CRM numbers are public records used to validate medical licensing.
    /// </remarks>
    /// <response code="200">CRM registration details retrieved successfully.</response>
    /// <response code="400">Invalid CRM registration ID format.</response>
    /// <response code="404">CRM registration not found.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DoctorCRMSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCRMByIdAsync(Guid id)
    {
        var crm = await _doctorCRMService.GetCRMByIdAsync(id);
        return Ok(crm);
    }

    /// <summary>
    /// Retrieves CRM registration information by CRM number and state combination.
    /// </summary>
    /// <param name="crmNumber">The CRM number to search for.</param>
    /// <param name="state">The Brazilian state abbreviation where the CRM is registered (e.g., SP, RJ, MG).</param>
    /// <returns>Detailed information about the CRM registration matching the specified number and state.</returns>
    /// <remarks>
    /// CRM (Conselho Regional de Medicina) is a Brazilian medical professional licensing system.
    /// Each state has its own regional medical council, and doctors must be registered in each state where they practice.
    /// **Search Parameters:**
    /// - CRM Number: The professional registration number (e.g., "123456")
    /// - State: Brazilian state abbreviation (e.g., "SP" for São Paulo, "RJ" for Rio de Janeiro)
    /// **Use Cases:**
    /// - Verifying doctor credentials before appointment booking
    /// - Cross-referencing medical professional registrations
    /// - Validating medical license authenticity
    /// - Regulatory compliance checks
    /// This endpoint is publicly accessible for credential verification purposes.
    /// </remarks>
    /// <response code="200">CRM registration information retrieved successfully.</response>
    /// <response code="400">Invalid CRM number format or missing state parameter.</response>
    /// <response code="404">No CRM registration found with the specified number and state combination.</response>
    [HttpGet("by-code")]
    [ProducesResponseType(typeof(DoctorCRMSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCRMByCodeAndState([FromQuery] string crmNumber, [FromQuery] string state)
    {
        var crm = await _doctorCRMService.GetCRMByCodeAndState(crmNumber, state);
        return Ok(crm);
    }

    /// <summary>
    /// Retrieves all CRM registrations in the system.
    /// </summary>
    /// <returns>A collection of all CRM registration records ordered by state and then by CRM number.</returns>
    /// <remarks>
    /// Returns a complete list of all CRM registrations including:
    /// - Doctor names and IDs
    /// - CRM numbers and registration states
    /// - Registration IDs for cross-referencing
    /// **Use Cases:**
    /// - Administrative oversight of medical licensing
    /// - Generating reports on registered medical professionals
    /// - System integration with regulatory bodies
    /// - Bulk verification of medical credentials
    /// **Data Ordering:**
    /// Results are sorted first by state (alphabetically), then by CRM number (numerically) for consistent navigation.
    /// **Access Control:**
    /// This endpoint is publicly accessible as CRM registrations are public records required for medical practice transparency.
    /// </remarks>
    /// <response code="200">CRM registration list retrieved successfully.</response>
    /// <response code="404">No CRM registrations found in the system.</response>
    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<DoctorCRMSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllCRMAsync()
    {
        var crms = await _doctorCRMService.GetAllCRMAsync();
        return Ok(crms);
    }

    /// <summary>
    /// Creates a new CRM registration for a doctor.
    /// </summary>
    /// <param name="doctorCRMDto">The CRM registration details including doctor ID, CRM number, and state.</param>
    /// <returns>Created response with location header pointing to the newly created CRM registration.</returns>
    /// <remarks>
    /// Sample request:
    ///     POST /api/v1/doctorcrm
    ///     {
    ///         "doctorId": "123e4567-e89b-12d3-a456-426614174000",
    ///         "crmNumber": "123456",
    ///         "state": "SP"
    ///     }
    /// **Business Rules:**
    /// - Each CRM number must be unique within its state
    /// - The doctor must exist in the system before creating a CRM registration
    /// - State abbreviations must be valid Brazilian state codes
    /// - A doctor can have multiple CRM registrations in different states
    /// **Validation Requirements:**
    /// - Doctor ID must be a valid GUID of an existing doctor
    /// - CRM Number must be a valid format (typically numeric)
    /// - State must be a valid Brazilian state abbreviation (2 characters)
    /// **Use Cases:**
    /// - Registering doctors for practice in new states
    /// - Adding additional medical licenses for existing doctors
    /// - Compliance with regional medical council requirements
    /// - Enabling multi-state medical practice
    /// Only administrators and doctors should be able to create CRM registrations.
    /// </remarks>
    /// <response code="201">CRM registration created successfully.</response>
    /// <response code="400">Invalid request data, duplicate CRM number/state combination, or doctor not found.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions to create CRM registrations.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Doctor}")]
    public async Task<IActionResult> CreateCRMAsync([FromBody] DoctorCRMRegistrationDto doctorCRMDto)
    {
        await _doctorCRMService.CreateCRMAsync(doctorCRMDto);
        return CreatedAtAction(
            nameof(GetCRMByCodeAndState),
            new { crmNumber = doctorCRMDto.CRMNumber, state = doctorCRMDto.State },
            null);
    }
}