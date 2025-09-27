namespace HealthConnect.Api.Controllers.v1;

using HealthConnect.Application.Dtos.Speciality;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller responsible for managing medical specialities in the HealthConnect system.
/// Provides endpoints for creating, retrieving, and managing medical specialties that doctors can be associated with.
/// </summary>
/// <param name="specialityService">The speciality service for handling medical specialty business logic.</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class SpecialityController(ISpecialityService specialityService)
    : ControllerBase
{
    private readonly ISpecialityService _specialityService = specialityService;

    /// <summary>
    /// Retrieves detailed information about a specific medical speciality by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the medical speciality to retrieve.</param>
    /// <returns>Detailed information about the specified medical speciality.</returns>
    /// <remarks>
    /// Returns comprehensive speciality information including:
    /// - Speciality unique identifier (GUID)
    /// - Speciality name (e.g., "Cardiology", "Dermatology", "Pediatrics")
    /// **Use Cases:**
    /// - Doctor profile displays showing specialized fields
    /// - Patient search filters for finding specialists
    /// - Administrative management of medical specialties
    /// - Integration with appointment booking systems
    /// This endpoint is publicly accessible to allow patients to understand available medical specialties
    /// when searching for appropriate healthcare providers.
    /// </remarks>
    /// <response code="200">Medical speciality information retrieved successfully.</response>
    /// <response code="400">Invalid speciality ID format.</response>
    /// <response code="404">Medical speciality not found.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SpecialitySummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSpecialityById(Guid id)
    {
        var speciality = await _specialityService.GetSpecialityById(id);
        return Ok(speciality);
    }

    /// <summary>
    /// Retrieves detailed information about a specific medical speciality by its name.
    /// </summary>
    /// <param name="name">The name of the medical speciality to search for (case-insensitive).</param>
    /// <returns>Detailed information about the speciality matching the specified name.</returns>
    /// <remarks>
    /// Allows searching for medical specialities by their common names such as:
    /// - "Cardiology" - Heart and cardiovascular system specialists
    /// - "Dermatology" - Skin, hair, and nail specialists
    /// - "Pediatrics" - Children's healthcare specialists
    /// - "Orthopedics" - Bone, joint, and muscle specialists
    /// - "Neurology" - Nervous system specialists
    /// **Use Cases:**
    /// - Text-based search functionality in patient interfaces
    /// - Validation of speciality names during doctor registration
    /// - Integration with external medical systems using speciality names
    /// - Autocomplete functionality for speciality selection
    /// **Search Behavior:**
    /// - Search is case-insensitive for user convenience
    /// - Exact name matching is required (partial matches not supported)
    /// - Special characters and spaces are considered in the search
    /// This endpoint is publicly accessible to support patient-facing search functionality.
    /// </remarks>
    /// <response code="200">Medical speciality information retrieved successfully.</response>
    /// <response code="400">Invalid or missing speciality name.</response>
    /// <response code="404">No medical speciality found with the specified name.</response>
    [HttpGet("by-name/{name}")]
    [ProducesResponseType(typeof(SpecialitySummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSpecialityByName(string name)
    {
        var speciality = await _specialityService.GetSpecialityByName(name);
        return Ok(speciality);
    }

    /// <summary>
    /// Retrieves a complete list of all available medical specialities in the system.
    /// </summary>
    /// <returns>A collection of all medical specialities ordered alphabetically by name.</returns>
    /// <remarks>
    /// Returns comprehensive information about all medical specialities available in the HealthConnect system.
    /// **Common Medical Specialities Included:**
    /// - **Cardiology**: Heart and cardiovascular diseases
    /// - **Dermatology**: Skin, hair, and nail conditions
    /// - **Endocrinology**: Hormonal and metabolic disorders
    /// - **Gastroenterology**: Digestive system disorders
    /// - **Neurology**: Nervous system conditions
    /// - **Oncology**: Cancer diagnosis and treatment
    /// - **Orthopedics**: Bone, joint, and muscle conditions
    /// - **Pediatrics**: Children's healthcare
    /// - **Psychiatry**: Mental health conditions
    /// - **Pulmonology**: Lung and respiratory conditions
    /// **Use Cases:**
    /// - Populating dropdown menus for doctor speciality selection
    /// - Patient browsing interfaces for finding specialists
    /// - Administrative dashboards for healthcare management
    /// - Integration with external medical systems
    /// - Generating reports on available medical services
    /// - Appointment booking system category filtering
    /// **Data Ordering:**
    /// Results are sorted alphabetically by speciality name for consistent user experience and easier navigation.
    /// This endpoint is publicly accessible to support patient-facing discovery of available medical services.
    /// </remarks>
    /// <response code="200">Medical specialities list retrieved successfully.</response>
    /// <response code="404">No medical specialities found in the system.</response>
    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<SpecialitySummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllSpecialities()
    {
        var specialities = await _specialityService.GetAllSpecialities();
        return Ok(specialities);
    }

    /// <summary>
    /// Creates a new medical speciality in the system.
    /// </summary>
    /// <param name="specialityDto">The medical speciality registration details including the speciality name.</param>
    /// <returns>The newly created medical speciality with generated ID and confirmation details.</returns>
    /// <remarks>
    /// Sample request:
    ///     POST /api/v1/speciality
    ///     {
    ///         "name": "Rheumatology"
    ///     }
    /// **Business Rules:**
    /// - Speciality names must be unique (case-insensitive)
    /// - Names should follow medical terminology standards
    /// - Names should be descriptive and professionally recognized
    /// - Special characters and numbers are generally not allowed
    /// **Validation Requirements:**
    /// - Name is required and cannot be empty or whitespace
    /// - Name length should be between 3 and 100 characters
    /// - Name should contain only letters, spaces, and hyphens
    /// - Name should not contain consecutive spaces or special characters
    /// **Use Cases:**
    /// - Adding new medical specialties as healthcare services expand
    /// - System configuration for multi-specialty medical centers
    /// - Integration with external medical classification systems
    /// - Support for emerging medical subspecialties
    /// **Administrative Note:**
    /// Only system administrators should be able to create new medical specialities as this affects
    /// the overall structure of medical services and doctor categorization throughout the system.
    /// **Impact on System:**
    /// - New specialities become available for doctor profile assignments
    /// - Patients can search and filter by the new speciality
    /// - Appointment booking systems will include the new category
    /// - Reporting and analytics will include the new speciality data.
    /// </remarks>
    /// <response code="201">Medical speciality created successfully.</response>
    /// <response code="400">Invalid request data, duplicate speciality name, or validation failure.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - administrator access required.</response>
    [HttpPost]
    [ProducesResponseType(typeof(SpecialitySummaryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = $"{AppRoles.Admin}")]
    public async Task<IActionResult> CreateSpeciality([FromBody] SpecialityRegistrationDto specialityDto)
    {
        var createdSpeciality = await _specialityService.CreateSpeciality(specialityDto);
        return CreatedAtAction(nameof(GetSpecialityById), new { id = createdSpeciality.Id }, createdSpeciality);
    }
}