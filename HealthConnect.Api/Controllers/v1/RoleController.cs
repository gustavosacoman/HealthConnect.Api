namespace HealthConnect.Api.Controllers.v1;

using HealthConnect.Application.Dtos.Role;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller responsible for managing user roles and role-based access control in the HealthConnect system.
/// Provides endpoints for creating, retrieving, and managing user roles and role assignments.
/// </summary>
/// <param name="roleService">The role service for handling role management business logic.</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class RoleController(IRoleService roleService)
    : ControllerBase
{
    private readonly IRoleService _roleService = roleService;

    /// <summary>
    /// Retrieves detailed information about a specific role by its name.
    /// </summary>
    /// <param name="roleName">The name of the role to retrieve (case-insensitive).</param>
    /// <returns>Detailed information about the specified role including ID and name.</returns>
    /// <remarks>
    /// Returns role information for system-defined roles such as:
    /// - Admin: Full system administration privileges
    /// - Doctor: Medical professional access to patient data and appointments
    /// - Patient: Patient-specific access to own medical records and appointments
    /// **Use Cases:**
    /// - Verifying role existence before assignment
    /// - Administrative role management
    /// - System integration with role-based authorization
    /// - Role-based feature access control
    /// **Access Control:**
    /// This endpoint is restricted to administrators only as role information is sensitive for system security.
    /// </remarks>
    /// <response code="200">Role information retrieved successfully.</response>
    /// <response code="400">Invalid role name or missing parameter.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - administrator access required.</response>
    /// <response code="404">Role not found with the specified name.</response>
    [HttpGet("{roleName}")]
    [ProducesResponseType(typeof(RoleSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = $"{AppRoles.Admin}")]
    public async Task<IActionResult> GetRoleByName(string roleName)
    {
        var role = await _roleService.GetRoleByNameAsync(roleName);
        return Ok(role);
    }

    /// <summary>
    /// Retrieves a complete list of all available roles in the system.
    /// </summary>
    /// <returns>A collection of all system roles with their IDs, names, and descriptions.</returns>
    /// <remarks>
    /// Returns comprehensive information about all roles available in the HealthConnect system.
    /// **Standard System Roles:**
    /// - **Admin**: System administrators with full access to all resources and user management
    /// - **Doctor**: Medical professionals with access to patient records, appointments, and medical data
    /// - **Patient**: End users with access to their own medical records and appointment booking
    /// **Use Cases:**
    /// - User role assignment interfaces
    /// - Administrative dashboards showing role hierarchy
    /// - System configuration and role management
    /// - Integration with external identity providers
    /// - Audit and compliance reporting
    /// Results are typically ordered alphabetically by role name for consistent presentation.
    /// </remarks>
    /// <response code="200">Role list retrieved successfully.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - administrator access required.</response>
    /// <response code="404">No roles found in the system.</response>
    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<RoleSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = $"{AppRoles.Admin}")]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _roleService.GetAllRolesAsync();
        return Ok(roles);
    }

    /// <summary>
    /// Retrieves detailed information about a specific role by its unique identifier.
    /// </summary>
    /// <param name="roleId">The unique identifier of the role to retrieve.</param>
    /// <returns>Detailed information about the specified role including ID and name.</returns>
    /// <remarks>
    /// This endpoint provides role lookup by GUID when the role's unique identifier is known.
    /// Useful for system integrations and internal role referencing.
    /// **Use Cases:**
    /// - Cross-referencing role assignments with role details
    /// - System integration where role GUIDs are used as foreign keys
    /// - Administrative interfaces displaying role information
    /// - Audit trails requiring role identification
    /// **Performance Note:**
    /// GUID-based lookups are optimized for fast retrieval and are preferred for internal system operations.
    /// </remarks>
    /// <response code="200">Role information retrieved successfully.</response>
    /// <response code="400">Invalid role ID format.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - administrator access required.</response>
    /// <response code="404">Role not found with the specified ID.</response>
    [HttpGet("{roleId:guid}")]
    [ProducesResponseType(typeof(RoleSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = $"{AppRoles.Admin}")]
    public async Task<IActionResult> GetRoleById(Guid roleId)
    {
        var role = await _roleService.GetRoleByIdAsync(roleId);
        return Ok(role);
    }

    /// <summary>
    /// Retrieves all roles currently assigned to a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose roles are being retrieved.</param>
    /// <returns>A collection of all roles assigned to the specified user.</returns>
    /// <remarks>
    /// Returns all active role assignments for the specified user, enabling role-based access control verification.
    /// **Business Rules:**
    /// - Users must have at least one role assigned
    /// - Users cannot have both Doctor and Patient roles simultaneously
    /// - Admin role can be combined with other roles for elevated privileges
    /// **Use Cases:**
    /// - Authorization middleware role verification
    /// - User profile displays showing current permissions
    /// - Administrative user management interfaces
    /// - Audit logging for access control compliance
    /// - Dynamic UI rendering based on user capabilities
    /// **Security Note:**
    /// This endpoint reveals user permission levels and should be restricted to administrators only.
    /// </remarks>
    /// <response code="200">User roles retrieved successfully.</response>
    /// <response code="400">Invalid user ID format.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - administrator access required.</response>
    /// <response code="404">User not found or user has no assigned roles.</response>
    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<RoleSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = $"{AppRoles.Admin}")]
    public async Task<IActionResult> GetRolesForUser(Guid userId)
    {
        var roles = await _roleService.GetRolesForUserAsync(userId);
        return Ok(roles);
    }

    /// <summary>
    /// Creates a new role in the system.
    /// </summary>
    /// <param name="roleRegistration">The role registration details including the role name.</param>
    /// <returns>Created response with location header pointing to the newly created role.</returns>
    /// <remarks>
    /// Sample request:
    ///     POST /api/v1/role
    ///     {
    ///         "name": "Specialist"
    ///     }
    /// **Business Rules:**
    /// - Role names must be unique (case-insensitive)
    /// - Role names should follow naming conventions (Pascal case recommended)
    /// - Standard system roles (Admin, Doctor, Patient) should not be duplicated
    /// - Role names should be descriptive and relate to system functionality
    /// **Validation Requirements:**
    /// - Name is required and cannot be empty or whitespace
    /// - Name length should be between 3 and 50 characters
    /// - Name should contain only letters, numbers, and spaces
    /// - No special characters or symbols allowed
    /// **Use Cases:**
    /// - Creating custom roles for specific organizational needs
    /// - Adding specialized permission sets for different user types
    /// - System expansion to support new user categories
    /// - Integration with external role management systems
    /// **Security Consideration:**
    /// Only system administrators should be able to create new roles as this affects system-wide access control.
    /// </remarks>
    /// <response code="201">Role created successfully.</response>
    /// <response code="400">Invalid request data, duplicate role name, or validation failure.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - administrator access required.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = $"{AppRoles.Admin}")]
    public async Task<IActionResult> CreateRole([FromBody] RoleRegistrationDto roleRegistration)
    {
        await _roleService.CreateRoleAsync(roleRegistration);
        return CreatedAtAction(nameof(GetRoleByName), new { roleName = roleRegistration.Name }, null);
    }
}