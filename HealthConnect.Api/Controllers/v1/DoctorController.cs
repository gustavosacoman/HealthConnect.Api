namespace HealthConnect.Api.Controllers.v1;

using HealthConnect.Application.Dtos.Doctors;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller responsible for managing doctor operations in the HealthConnect system.
/// Provides endpoints for retrieving, updating, and searching doctor information with various levels of detail and access control.
/// </summary>
/// <param name="doctorService">The doctor service for handling medical professional business logic.</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class DoctorController(IDoctorService doctorService)
    : ControllerBase
{
    private readonly IDoctorService _doctorService = doctorService;

    /// <summary>
    /// Retrieves summary information for a specific doctor by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the doctor to retrieve.</param>
    /// <returns>Summary information about the specified doctor including specialities and CRM registrations.</returns>
    /// <remarks>
    /// Returns basic doctor information including:
    /// - Personal details (name, sex, biography)
    /// - Medical specialities with RQE numbers
    /// - CRM (Regional Medical Council) registrations
    /// This endpoint is publicly accessible to allow patients to view doctor profiles when searching for medical care.
    /// Personal contact information is not included in the summary view for privacy reasons.
    /// </remarks>
    /// <response code="200">Doctor summary information retrieved successfully.</response>
    /// <response code="400">Invalid doctor ID format.</response>
    /// <response code="404">Doctor not found.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DoctorSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDoctorById(Guid id)
    {
        var doctor = await _doctorService.GetDoctorByIdAsync(id);
        return Ok(doctor);
    }

    /// <summary>
    /// Retrieves detailed information for a specific doctor by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the doctor to retrieve detailed information for.</param>
    /// <returns>Comprehensive detailed information about the specified doctor.</returns>
    /// <remarks>
    /// This endpoint provides the most comprehensive doctor information available, including:
    /// - Complete personal details (name, email, phone, CPF, birth date, sex)
    /// - Medical specialities with RQE numbers
    /// - CRM (Regional Medical Council) registrations with states
    /// - Professional biography
    /// - System identifiers (doctor ID, user ID)
    /// **Access Restrictions:**
    /// - Administrators: Full access to all doctor details
    /// - Doctors: Can access their own detailed information and colleagues' details
    /// **Security Note:**
    /// This endpoint contains sensitive personal information and is restricted to authenticated users with appropriate roles.
    /// </remarks>
    /// <response code="200">Detailed doctor information retrieved successfully.</response>
    /// <response code="400">Invalid doctor ID format.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions to access detailed doctor information.</response>
    /// <response code="404">Doctor not found.</response>
    [HttpGet("detail/{id:guid}")]
    [ProducesResponseType(typeof(DoctorDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Doctor}")]
    public async Task<IActionResult> GetDoctorByIdDetail(Guid id)
    {
        var doctor = await _doctorService.GetDoctorByIdDetailAsync(id);
        return Ok(doctor);
    }

    /// <summary>
    /// Retrieves doctor information by their RQE (Registro de Qualificação de Especialista) number.
    /// </summary>
    /// <param name="rqe">The RQE number of the doctor to search for.</param>
    /// <returns>Summary information about the doctor with the specified RQE number.</returns>
    /// <remarks>
    /// RQE (Registro de Qualificação de Especialista) is a Brazilian medical specialist qualification registry number.
    /// This endpoint allows searching for doctors by their professional registration number.
    /// **Use Cases:**
    /// - Verifying doctor credentials by RQE number
    /// - Cross-referencing medical professional registrations
    /// - Patient verification of doctor qualifications
    /// This endpoint is publicly accessible to allow verification of medical professional credentials.
    /// </remarks>
    /// <response code="200">Doctor information retrieved successfully.</response>
    /// <response code="400">Invalid RQE format or missing parameter.</response>
    /// <response code="404">No doctor found with the specified RQE number.</response>
    [HttpGet("by-rqe/{rqe}")]
    [ProducesResponseType(typeof(DoctorSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDoctorByRQE(string rqe)
    {
        var doctor = await _doctorService.GetDoctorByRQEAsync(rqe);
        return Ok(doctor);
    }

    /// <summary>
    /// Retrieves summary information for all doctors in the system.
    /// </summary>
    /// <returns>A collection of doctor summary information for all registered doctors.</returns>
    /// <remarks>
    /// Returns a complete list of all active doctors with summary information including:
    /// - Basic personal details (name, sex, biography)
    /// - Medical specialities and RQE numbers
    /// - CRM registrations and states
    /// **Use Cases:**
    /// - Doctor directory for patient browsing
    /// - Administrative overview of medical staff
    /// - Integration with appointment booking systems
    /// Results are typically ordered alphabetically by doctor name for easier navigation.
    /// This endpoint is publicly accessible to allow patients to browse available doctors.
    /// </remarks>
    /// <response code="200">Doctor list retrieved successfully.</response>
    /// <response code="404">No doctors found in the system.</response>
    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<DoctorSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllDoctors()
    {
        var doctors = await _doctorService.GetAllDoctorsAsync();
        return Ok(doctors);
    }

    /// <summary>
    /// Retrieves all doctors who specialize in a specific medical speciality.
    /// </summary>
    /// <param name="specialityId">The unique identifier of the medical speciality to filter by.</param>
    /// <returns>A collection of detailed doctor information for all doctors with the specified speciality.</returns>
    /// <remarks>
    /// Returns detailed information for all doctors who have the specified medical speciality, including:
    /// - Complete personal and professional details
    /// - All specialities (including the filtered one)
    /// - CRM registrations
    /// - Professional biographies
    /// **Use Cases:**
    /// - Finding specialists for specific medical conditions
    /// - Filtering doctors by medical expertise
    /// - Appointment booking systems filtering by speciality
    /// - Healthcare network management
    /// This endpoint is publicly accessible to help patients find appropriate medical specialists.
    /// Results include detailed information to help patients make informed choices.
    /// </remarks>
    /// <response code="200">Specialist doctors retrieved successfully.</response>
    /// <response code="400">Invalid speciality ID format.</response>
    /// <response code="404">No doctors found for the specified speciality or speciality not found.</response>
    [HttpGet("by-Speciality/all/{specialityId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<DoctorDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllDoctorsBySpeciality(Guid specialityId)
    {
        var doctors = await _doctorService.GetAllDoctorsBySpecialityAsync(specialityId);
        return Ok(doctors);
    }

    /// <summary>
    /// Retrieves detailed doctor information by the associated user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user associated with the doctor.</param>
    /// <returns>Detailed information about the doctor associated with the specified user.</returns>
    /// <remarks>
    /// This endpoint is useful when you have a user ID (from authentication context) and need to retrieve
    /// the complete doctor profile information. Returns comprehensive doctor details including:
    /// - Complete personal information (name, email, phone, CPF, birth date, sex)
    /// - Professional details (specialities, CRM registrations, biography)
    /// - System identifiers (doctor ID, user ID)
    /// **Use Cases:**
    /// - Doctor profile retrieval after authentication
    /// - Cross-referencing user accounts with doctor records
    /// - Self-service doctor profile management
    /// - Administrative user-doctor relationship management
    /// **Access Control:**
    /// - Administrators: Can access any doctor's detailed information
    /// - Doctors: Can access their own detailed information.
    /// </remarks>
    /// <response code="200">Detailed doctor information retrieved successfully.</response>
    /// <response code="400">Invalid user ID format.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions to access this doctor's information.</response>
    /// <response code="404">Doctor not found for the specified user ID.</response>
    [HttpGet("detail/by-userid/{userId:guid}")]
    [ProducesResponseType(typeof(DoctorDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Doctor}")]
    public async Task<IActionResult> GetDoctorByUserId(Guid userId)
    {
        var doctor = await _doctorService.GetDoctoDetailByUserIdAsync(userId);
        return Ok(doctor);
    }

    /// <summary>
    /// Updates an existing doctor's professional information.
    /// </summary>
    /// <param name="id">The unique identifier of the doctor to update.</param>
    /// <param name="data">The doctor update details including RQE, CRM, speciality, and biography.</param>
    /// <returns>Updated summary information about the doctor.</returns>
    /// <remarks>
    /// Sample request:
    ///     PATCH /api/v1/doctor?id=123e4567-e89b-12d3-a456-426614174000
    ///     {
    ///         "rqe": "RQE-12345",
    ///         "crm": "CRM-54321",
    ///         "specialityId": "789e0123-e89b-12d3-a456-426614174000",
    ///         "biography": "Updated professional biography with new qualifications and experience."
    ///     }
    /// **Updateable Fields:**
    /// - RQE (Registro de Qualificação de Especialista) number
    /// - CRM (Conselho Regional de Medicina) number
    /// - Primary speciality ID
    /// - Professional biography
    /// **Business Rules:**
    /// - RQE numbers must be unique across the system
    /// - CRM numbers must be valid and unique per state
    /// - Speciality must exist in the system
    /// - Only administrators and the doctor themselves can update professional information
    /// **Note:** Personal information (name, email, etc.) should be updated through the User endpoints.
    /// </remarks>
    /// <response code="200">Doctor information updated successfully.</response>
    /// <response code="400">Invalid request data, duplicate RQE/CRM, or business rule violation.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions to update this doctor.</response>
    /// <response code="404">Doctor or speciality not found.</response>
    [HttpPatch]
    [ProducesResponseType(typeof(DoctorSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Doctor}")]
    public async Task<IActionResult> UpdateDoctor([FromQuery] Guid id, [FromBody] DoctorUpdatingDto data)
    {
        var updatedDoctor = await _doctorService.UpdateDoctorAsync(id, data);
        return Ok(updatedDoctor);
    }
}