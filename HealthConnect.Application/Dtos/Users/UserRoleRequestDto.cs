namespace HealthConnect.Application.Dtos.Users;

/// <summary>
/// Data transfer object for requesting a user role assignment.
/// </summary>
public class UserRoleRequestDto
{
    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    required public string Email { get; set; }

    /// <summary>
    /// Gets or sets the name of the role to assign.
    /// </summary>
    required public string RoleName { get; set; }
}
