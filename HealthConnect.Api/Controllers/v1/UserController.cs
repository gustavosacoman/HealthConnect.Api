namespace HealthConnect.Api.Controllers.v1;

using HealthConnect.Application.Dtos.Client;
using HealthConnect.Application.Dtos.Doctors;
using HealthConnect.Application.Dtos.Users;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller responsible for managing user accounts and user-related operations in the HealthConnect system.
/// Provides endpoints for user creation, retrieval, updating, deletion, and role management across all user types.
/// </summary>
/// <param name="userService">The user service for handling user account business logic.</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class UserController(IUserService userService)
    : ControllerBase
{
    private readonly IUserService _userService = userService;

    /// <summary>
    /// Retrieves summary information for a specific user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve.</param>
    /// <returns>Summary information about the specified user including basic profile details.</returns>
    /// <remarks>
    /// Returns basic user account information including:
    /// - User ID and personal details (name, email, phone)
    /// - CPF (Brazilian tax ID) and birth date
    /// - Account status and creation information
    /// **Access Control:**
    /// - Administrators: Can access any user's information
    /// - Doctors: Can access their own information and patient information for their appointments
    /// - Patients: Can access their own information
    /// **Security Note:**
    /// Access to user information is controlled at the service level based on the authenticated user's context and role.
    /// Sensitive information like passwords and internal system data is never included in the response.
    /// </remarks>
    /// <response code="200">User information retrieved successfully.</response>
    /// <response code="400">Invalid user ID format.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions to access this user.</response>
    /// <response code="404">User not found.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Doctor},{AppRoles.Patient}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return Ok(user);
    }

    /// <summary>
    /// Retrieves user information by their email address.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve.</param>
    /// <returns>Summary information about the user with the specified email address.</returns>
    /// <remarks>
    /// Allows user lookup by email address, which is the primary authentication identifier in the system.
    /// **Use Cases:**
    /// - User profile retrieval after authentication
    /// - Email-based user search for administrative purposes
    /// - Account verification and password reset workflows
    /// - Cross-referencing user accounts in support scenarios
    /// **Access Control:**
    /// Similar to ID-based lookup, access is controlled based on user roles and relationship to the requested user.
    /// **Email Format:**
    /// Email addresses are case-insensitive and must be valid email format (validated at service level).
    /// </remarks>
    /// <response code="200">User information retrieved successfully.</response>
    /// <response code="400">Invalid email format or missing parameter.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions to access this user.</response>
    /// <response code="404">User not found with the specified email address.</response>
    [HttpGet("by-email/{email}")]
    [ProducesResponseType(typeof(UserSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Doctor},{AppRoles.Patient}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        var user = await _userService.GetUserByEmailAsync(email);
        return Ok(user);
    }

    /// <summary>
    /// Retrieves summary information for all users in the system.
    /// </summary>
    /// <returns>A collection of user summary information for all registered users.</returns>
    /// <remarks>
    /// Returns a complete list of all active users with summary information including:
    /// - Basic identification and contact details
    /// - Account creation and status information
    /// - User roles and permissions (summary level)
    /// **Administrative Use Only:**
    /// This endpoint is restricted to administrators only due to privacy and security considerations.
    /// It provides system-wide user management capabilities.
    /// **Use Cases:**
    /// - Administrative dashboards and user management interfaces
    /// - System analytics and reporting
    /// - Bulk user operations and maintenance
    /// - User account auditing and compliance
    /// **Data Ordering:**
    /// Results are typically ordered alphabetically by user name for consistent navigation.
    /// **Performance Note:**
    /// This endpoint may return large datasets and should be used judiciously.
    /// Consider implementing pagination for production systems with many users.
    /// </remarks>
    /// <response code="200">User list retrieved successfully.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - administrator access required.</response>
    /// <response code="404">No users found in the system.</response>
    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<UserSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = $"{AppRoles.Admin}")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    /// <summary>
    /// Creates a new doctor user account with complete medical professional profile.
    /// </summary>
    /// <param name="data">The complete doctor registration data including personal, medical, and credential information.</param>
    /// <returns>Detailed information about the created doctor including user account and medical profile.</returns>
    /// <remarks>
    /// Sample request:
    ///     POST /api/v1/user/doctor
    ///     {
    ///         "name": "Dr. João Silva",
    ///         "email": "joao.silva@hospital.com",
    ///         "phone": "+5511999999999",
    ///         "password": "SecurePassword123!",
    ///         "cpf": "12345678901",
    ///         "speciality": "Cardiology",
    ///         "birthDate": "1980-05-15",
    ///         "rqe": "RQE-12345",
    ///         "crm": "123456",
    ///         "crmState": "SP",
    ///         "biography": "Experienced cardiologist with 15 years of practice...",
    ///         "sex": "Male"
    ///     }
    /// **Complete Registration Process:**
    /// This endpoint creates:
    /// - User account with authentication credentials
    /// - Doctor profile with medical specialities
    /// - CRM registration for medical licensing
    /// - RQE (specialist qualification) registration
    /// - Automatic assignment of "Doctor" role
    /// **Business Rules:**
    /// - Email addresses must be unique across the system
    /// - RQE numbers must be unique and valid
    /// - CRM numbers must be unique per state
    /// - Speciality must exist in the system
    /// - Password must meet security requirements
    /// - CPF must be valid Brazilian format
    /// **Validation Requirements:**
    /// - All required fields must be provided
    /// - Email must be valid format
    /// - Phone number should be valid international format
    /// - Birth date must indicate user is at least 18 years old
    /// - CRM state must be valid Brazilian state abbreviation
    /// This endpoint is publicly accessible to allow doctor registration, but may require approval workflows in production.
    /// </remarks>
    /// <response code="201">Doctor account created successfully.</response>
    /// <response code="400">Invalid request data, duplicate email/RQE/CRM, or validation failure.</response>
    /// <response code="404">Speciality not found or system role configuration error.</response>
    [HttpPost("doctor")]
    [ProducesResponseType(typeof(DoctorDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateDoctor([FromBody] DoctorRegistrationDto data)
    {
        var doctor = await _userService.CreateDoctorAsync(data);
        return CreatedAtAction(nameof(GetUserById), new { id = doctor.UserId }, doctor);
    }

    /// <summary>
    /// Creates a new patient/client user account with complete personal profile.
    /// </summary>
    /// <param name="data">The complete client registration data including personal information and credentials.</param>
    /// <returns>Detailed information about the created client including user account and personal profile.</returns>
    /// <remarks>
    /// Sample request:
    ///     POST /api/v1/user/client
    ///     {
    ///         "name": "Maria Santos",
    ///         "email": "maria.santos@email.com",
    ///         "phone": "+5511888888888",
    ///         "password": "SecurePassword123!",
    ///         "cpf": "98765432100",
    ///         "birthDate": "1990-03-20",
    ///         "sex": "Female"
    ///     }
    /// **Complete Registration Process:**
    /// This endpoint creates:
    /// - User account with authentication credentials
    /// - Client/Patient profile for medical records
    /// - Automatic assignment of "Patient" role
    /// - Account setup for appointment booking and medical history
    /// **Business Rules:**
    /// - Email addresses must be unique across the system
    /// - CPF must be valid and unique Brazilian tax ID
    /// - Password must meet security requirements
    /// - Phone number is optional but recommended for notifications
    /// **Validation Requirements:**
    /// - All required fields must be provided
    /// - Email must be valid format and not already registered
    /// - CPF must be valid Brazilian format (11 digits)
    /// - Birth date must be valid and indicate user is at least 13 years old
    /// - Password must meet minimum security requirements (length, complexity)
    /// **Use Cases:**
    /// - Patient self-registration for healthcare services
    /// - Administrative creation of patient accounts
    /// - Integration with external patient management systems
    /// - Bulk patient import processes
    /// This endpoint is publicly accessible to allow patient self-registration.
    /// </remarks>
    /// <response code="201">Client account created successfully.</response>
    /// <response code="400">Invalid request data, duplicate email/CPF, or validation failure.</response>
    /// <response code="404">System role configuration error.</response>
    [HttpPost("client")]
    [ProducesResponseType(typeof(ClientDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateClient([FromBody] ClientRegistrationDto data)
    {
        var client = await _userService.CreateClientAsync(data);
        return CreatedAtAction(nameof(GetUserById), new { id = client.UserId }, client);
    }

    /// <summary>
    /// Updates an existing user's account information.
    /// </summary>
    /// <param name="id">The unique identifier of the user to update.</param>
    /// <param name="data">The user update data containing fields to be modified.</param>
    /// <returns>Updated summary information about the user.</returns>
    /// <remarks>
    /// Sample request:
    ///     PATCH /api/v1/user/123e4567-e89b-12d3-a456-426614174000
    ///     {
    ///         "name": "Updated Name",
    ///         "phone": "+5511777777777",
    ///         "password": "NewSecurePassword123!"
    ///     }
    /// **Updateable Fields:**
    /// - **Name**: User's display name
    /// - **Phone**: Contact phone number
    /// - **Password**: Account password (automatically hashed)
    /// **Fields NOT Updateable via this endpoint:**
    /// - Email (requires separate verification process)
    /// - CPF (immutable for legal/audit reasons)
    /// - Birth Date (immutable for verification purposes)
    /// - Sex (requires separate process for privacy/legal reasons)
    /// **Update Behavior:**
    /// - Only provided fields are updated (partial updates supported)
    /// - Null or empty values are ignored (existing values preserved)
    /// - Password updates automatically generate new salt and hash
    /// **Access Control:**
    /// - Administrators: Can update any user account
    /// - Doctors: Can update their own account
    /// - Patients: Can update their own account
    /// - Cross-user updates are prevented at service level
    /// **Security Notes:**
    /// - Password changes invalidate existing authentication tokens
    /// - All updates are logged for audit purposes
    /// - Sensitive operations may require additional verification.
    /// </remarks>
    /// <response code="200">User information updated successfully.</response>
    /// <response code="400">Invalid request data or user ID format.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions to update this user.</response>
    /// <response code="404">User not found.</response>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(UserSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Doctor},{AppRoles.Patient}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserUpdatingDto data)
    {
        var user = await _userService.UpdateUserAsync(id, data);
        return Ok(user);
    }

    /// <summary>
    /// Performs a soft delete of a user account by their email address.
    /// </summary>
    /// <param name="email">The email address of the user to delete.</param>
    /// <returns>No content response indicating successful deletion.</returns>
    /// <remarks>
    /// **Soft Delete Implementation:**
    /// This endpoint performs a "soft delete" which means:
    /// - User account is marked as deleted but data is preserved
    /// - Account becomes inaccessible for login and normal operations
    /// - Associated medical records and appointments are preserved for legal/audit requirements
    /// - Data can be recovered by administrators if needed
    /// **Cascade Effects:**
    /// When a user is deleted, the following related data is also soft-deleted:
    /// - Client profile (if user is a patient)
    /// - Doctor profile (if user is a doctor)
    /// - Active appointments may be cancelled
    /// - Access permissions are immediately revoked
    /// **Business Rules:**
    /// - Administrators can delete any user account
    /// - Users can request deletion of their own account
    /// - Doctors with active appointments may require special handling
    /// - Some user types may have restrictions based on business requirements
    /// **Legal Considerations:**
    /// - Medical data retention laws may apply
    /// - User data privacy rights (LGPD/GDPR compliance)
    /// - Audit trail requirements for healthcare systems
    /// **Security Note:**
    /// This is a high-impact operation that should be carefully controlled and logged.
    /// Consider implementing additional verification steps for production systems.
    /// **Alternative Approaches:**
    /// - Account deactivation (temporary suspension)
    /// - Data anonymization for privacy compliance
    /// - Hard delete for complete data removal (where legally permitted).
    /// </remarks>
    /// <response code="204">User account deleted successfully.</response>
    /// <response code="400">Invalid email format.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - insufficient permissions to delete this user.</response>
    /// <response code="404">User not found with the specified email.</response>
    [HttpDelete("{email}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Doctor},{AppRoles.Patient}")]
    public async Task<IActionResult> DeleteUser(string email)
    {
        await _userService.DeleteUserAsync(email);
        return NoContent();
    }

    /// <summary>
    /// Adds a role assignment to a specific user account.
    /// </summary>
    /// <param name="userRoleRequestDto">The request containing user email and role name to assign.</param>
    /// <returns>No content response indicating successful role assignment.</returns>
    /// <remarks>
    /// Sample request:
    ///     POST /api/v1/user/add-role
    ///     {
    ///         "email": "user@example.com",
    ///         "roleName": "Doctor"
    ///     }
    /// **Available Roles:**
    /// - **Admin**: Full system administration privileges
    /// - **Doctor**: Medical professional access to patient data and appointments
    /// - **Patient**: Patient-specific access to own medical records and appointments
    /// **Business Rules:**
    /// - Users cannot have both "Doctor" and "Patient" roles simultaneously
    /// - "Admin" role can be combined with other roles for elevated privileges
    /// - Users must have at least one role at all times
    /// - Role names are case-insensitive
    /// **Use Cases:**
    /// - Promoting users to administrative roles
    /// - Granting additional permissions for specific scenarios
    /// - Role-based access control management
    /// - System integration with external identity providers
    /// **Access Control:**
    /// Only system administrators can assign roles to users due to security implications.
    /// **Validation:**
    /// - User must exist in the system
    /// - Role must exist in the system
    /// - User cannot already have the specified role
    /// - Role combination rules are enforced
    /// **Security Considerations:**
    /// Role assignments immediately affect user permissions and should be carefully managed.
    /// All role changes should be logged for audit purposes.
    /// </remarks>
    /// <response code="204">Role successfully assigned to user.</response>
    /// <response code="400">Invalid request data, duplicate role assignment, or business rule violation.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - administrator access required.</response>
    /// <response code="404">User or role not found.</response>
    [HttpPost("add-role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = $"{AppRoles.Admin}")]
    public async Task<IActionResult> AddRoleToUser([FromBody] UserRoleRequestDto userRoleRequestDto)
    {
        await _userService.AddRoleLinkToUserAsync(userRoleRequestDto);
        return NoContent();
    }

    /// <summary>
    /// Removes a role assignment from a specific user account.
    /// </summary>
    /// <param name="userRoleRequestDto">The request containing user email and role name to remove.</param>
    /// <returns>No content response indicating successful role removal.</returns>
    /// <remarks>
    /// Sample request:
    ///     DELETE /api/v1/user/remove-role
    ///     {
    ///         "email": "user@example.com",
    ///         "roleName": "Doctor"
    ///     }
    /// **Business Rules:**
    /// - Users must retain at least one role (cannot remove all roles)
    /// - Role names are case-insensitive
    /// - User must currently have the specified role to remove it
    /// **Use Cases:**
    /// - Revoking elevated privileges when no longer needed
    /// - Role-based access control management
    /// - User account cleanup and maintenance
    /// - Compliance with least-privilege security principles
    /// **Access Control:**
    /// Only system administrators can remove roles from users due to security implications.
    /// **Validation:**
    /// - User must exist in the system
    /// - Role must exist in the system
    /// - User must currently have the specified role
    /// - User must have at least one other role after removal
    /// **Security Considerations:**
    /// - Role removals immediately affect user permissions
    /// - Users may lose access to data and functionality
    /// - Consider notifying affected users of permission changes
    /// - All role changes should be logged for audit purposes
    /// **Impact Analysis:**
    /// Before removing roles, consider:
    /// - Active user sessions and cached permissions
    /// - Dependencies on role-specific functionality
    /// - Business processes that may be affected
    /// - Regulatory compliance requirements.
    /// </remarks>
    /// <response code="204">Role successfully removed from user.</response>
    /// <response code="400">Invalid request data, user doesn't have role, or business rule violation.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - administrator access required.</response>
    /// <response code="404">User, role, or role assignment not found.</response>
    [HttpDelete("remove-role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = $"{AppRoles.Admin}")]
    public async Task<IActionResult> RemoveRoleFromUser([FromBody] UserRoleRequestDto userRoleRequestDto)
    {
        await _userService.RemoveRoleLinkFromUserAsync(userRoleRequestDto);
        return NoContent();
    }
}