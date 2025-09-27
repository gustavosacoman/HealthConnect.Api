namespace HealthConnect.Application.Interfaces.RepositoriesInterfaces;

using HealthConnect.Domain.Models.Roles;

/// <summary>
/// Provides methods for managing roles and user-role associations.
/// </summary>
public interface IRoleRepository
{
    /// <summary>
    /// Gets the roles assigned to a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A collection of roles for the user.</returns>
    Task<IEnumerable<Role>> GetRolesForUserAsync(Guid userId);

    /// <summary>
    /// Gets all roles in the system.
    /// </summary>
    /// <returns>A collection of all roles.</returns>
    Task<IEnumerable<Role>> GetAllRolesAsync();

    /// <summary>
    /// Gets a role by its name.
    /// </summary>
    /// <param name="roleName">The name of the role.</param>
    /// <returns>The role if found; otherwise, null.</returns>
    Task<Role?> GetRoleByNameAsync(string roleName);

    /// <summary>
    /// Gets a role by its unique identifier.
    /// </summary>
    /// <param name="roleId">The unique identifier of the role.</param>
    /// <returns>The role if found; otherwise, null.</returns>
    Task<Role?> GetRoleByIdAsync(Guid roleId);

    /// <summary>
    /// Creates a new role.
    /// </summary>
    /// <param name="role">The role to create.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task CreateRoleAsync(Role role);

    /// <summary>
    /// Associates a user with a role.
    /// </summary>
    /// <param name="userRole">The user-role association to create.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task CreateUserRoleAsync(UserRole userRole);
}
