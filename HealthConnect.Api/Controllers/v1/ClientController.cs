namespace HealthConnect.Api.Controllers.v1;

using HealthConnect.Application.Dtos.Client;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller responsible for managing client (patient) operations in the HealthConnect system.
/// Provides endpoints for retrieving client information with various levels of detail and access control.
/// </summary>
/// <param name="clientService">The client service for handling patient-related business logic.</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class ClientController(IClientService clientService)
    : ControllerBase
{
    private readonly IClientService _clientService = clientService;

    /// <summary>
    /// Retrieves summary information for all clients in the system.
    /// </summary>
    /// <returns>A collection of client summary information including basic identification and contact details.</returns>
    /// <remarks>
    /// Returns a complete list of all clients with summary information including:
    /// - Client ID and User ID
    /// - Full name
    /// - Email address
    /// - Sex/Gender
    /// This endpoint is restricted to administrators only for system management purposes.
    /// Results are ordered alphabetically by client name for easier navigation.
    /// </remarks>
    /// <response code="200">Client list retrieved successfully.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - administrator access required.</response>
    /// <response code="404">No clients found in the system.</response>
    [ProducesResponseType(typeof(IEnumerable<ClientSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("all")]
    [Authorize(Roles = $"{AppRoles.Admin}")]
    public async Task<IActionResult> GetAllClientsAsync()
    {
        var clients = await _clientService.GetAllClientsAsync();
        return Ok(clients);
    }

    /// <summary>
    /// Retrieves summary information for a specific client by their client ID.
    /// </summary>
    /// <param name="id">The unique identifier of the client to retrieve.</param>
    /// <returns>Summary information about the specified client.</returns>
    /// <remarks>
    /// Returns basic client information including identification and contact details.
    /// This endpoint provides a lighter payload compared to the detailed client endpoint.
    /// **Access Control:**
    /// - Administrators: Can access any client's information
    /// - Doctors: Can access client information for their patients
    /// - Patients: Can access their own information
    /// The actual authorization filtering may be handled at the service level based on the authenticated user's context.
    /// </remarks>
    /// <response code="200">Client information retrieved successfully.</response>
    /// <response code="400">Invalid client ID format.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions to access this client.</response>
    /// <response code="404">Client not found.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ClientSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Doctor},{AppRoles.Patient}")]
    public async Task<IActionResult> GetClientByIdAsync(Guid id)
    {
        var client = await _clientService.GetClientByIdAsync(id);
        return Ok(client);
    }

    /// <summary>
    /// Retrieves detailed client information by the associated user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user associated with the client.</param>
    /// <returns>Detailed information about the client associated with the specified user.</returns>
    /// <remarks>
    /// This endpoint is useful when you have a user ID (from authentication context) and need to retrieve
    /// the complete client profile information. Returns comprehensive client details including:
    /// - Personal information (name, email, phone, CPF)
    /// - Demographic data (birth date, sex)
    /// - System identifiers (client ID, user ID)
    /// **Use Cases:**
    /// - User profile retrieval after authentication
    /// - Cross-referencing user accounts with client records
    /// - Detailed client information for medical records
    /// **Access Control:**
    /// Similar to other client endpoints, access is controlled based on user roles and relationship to the client.
    /// </remarks>
    /// <response code="200">Detailed client information retrieved successfully.</response>
    /// <response code="400">Invalid user ID format.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions to access this client.</response>
    /// <response code="404">Client not found for the specified user ID.</response>
    [ProducesResponseType(typeof(ClientDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("user/{userId:guid}")]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Doctor},{AppRoles.Patient}")]
    public async Task<IActionResult> GetClientByUserIdAsync(Guid userId)
    {
        var client = await _clientService.GetClientByUserIdAsync(userId);
        return Ok(client);
    }

    /// <summary>
    /// Retrieves comprehensive detailed information for a specific client by their client ID.
    /// </summary>
    /// <param name="id">The unique identifier of the client to retrieve detailed information for.</param>
    /// <returns>Complete detailed information about the specified client.</returns>
    /// <remarks>
    /// This endpoint provides the most comprehensive client information available, including:
    /// - Complete personal details (name, email, phone, CPF)
    /// - Demographic information (birth date, sex)
    /// - System identifiers (client ID, user ID)
    /// - All available profile information
    /// **Intended Usage:**
    /// - Patient profile management
    /// - Medical record access
    /// - Administrative client management
    /// - Detailed patient information for healthcare providers
    /// **Access Restrictions:**
    /// - Administrators: Full access to all client details
    /// - Patients: Can only access their own detailed information
    /// - Doctors: Access may be restricted based on patient-doctor relationships
    /// **Security Note:**
    /// This endpoint contains sensitive personal information and should be used carefully with appropriate access controls.
    /// </remarks>
    /// <response code="200">Detailed client information retrieved successfully.</response>
    /// <response code="400">Invalid client ID format.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions to access detailed client information.</response>
    /// <response code="404">Client not found.</response>
    [ProducesResponseType(typeof(ClientDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("detail/{id:guid}")]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Patient}")]
    public async Task<IActionResult> GetClientDetailByIdAsync(Guid id)
    {
        var client = await _clientService.GetClientDetailByIdAsync(id);
        return Ok(client);
    }
}