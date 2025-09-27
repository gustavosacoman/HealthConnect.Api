namespace HealthConnect.Application.Interfaces.ServicesInterface;

using HealthConnect.Application.Dtos.Role;

/// <summary>
/// Provides role management operations.
/// </summary>
public interface IRoleService
{
    /// <summary>
    /// Gets a role by its name.
    /// </summary>
    /// <param name="roleName">The name of the role.</param>
    /// <returns>The role summary.</returns>
    Task<RoleSummaryDto> GetRoleByNameAsync(string roleName);

    /// <summary>
    /// Gets all roles.
    /// </summary>
    /// <returns>A collection of role summaries.</returns>
    Task<IEnumerable<RoleSummaryDto>> GetAllRolesAsync();

    /// <summary>
    /// Gets a role by its unique identifier.
    /// </summary>
    /// <param name="roleId">The unique identifier of the role.</param>
    /// <returns>The role summary.</returns>
    Task<RoleSummaryDto> GetRoleByIdAsync(Guid roleId);

    /// <summary>
    /// Gets all roles assigned to a user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A collection of role summaries.</returns>
    Task<IEnumerable<RoleSummaryDto>> GetRolesForUserAsync(Guid userId);

    /// <summary>
    /// Creates a new role.
    /// </summary>
    /// <param name="roleRegistration">The role registration details.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateRoleAsync(RoleRegistrationDto roleRegistration);
}
