namespace HealthConnect.Application.Interfaces.RepositoriesInterfaces;

using HealthConnect.Domain.Models;
using HealthConnect.Domain.Models.Roles;

/// <summary>
/// Provides methods for accessing and managing <see cref="User"/> entities.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <returns>A collection of all <see cref="User"/> entities.</returns>
    Task<IEnumerable<User>> GetAllUsersAsync();

    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>The <see cref="User"/> entity if found; otherwise, <c>null</c>.</returns>
    Task<User?> GetUserByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <returns>The <see cref="User"/> entity if found; otherwise, <c>null</c>.</returns>
    Task<User?> GetUserByEmailAsync(string email);

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="user">The <see cref="User"/> entity to create.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task CreateUserAsync(User user);

    /// <summary>
    /// Retrieves a doctor by their email address.
    /// </summary>
    /// <param name="email">The email address of the doctor.</param>
    /// <returns>The <see cref="User"/> entity if found; otherwise, <c>null</c>.</returns>
    Task<User?> GetDoctorByEmailAsync(string email);

    /// <summary>
    /// Removes a link between a user and a role.
    /// </summary>
    /// <param name="userRole">The <see cref="UserRole"/> link to remove.</param>
    void RemoveRoleLinkAsync(UserRole userRole);

    /// <summary>
    /// Adds a link between a user and a role.
    /// </summary>
    /// <param name="userRole">The <see cref="UserRole"/> link to add.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddUserRoleLinkAsync(UserRole userRole);

    /// <summary>
    /// Retrieves the link between a user and a role.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="roleId">The unique identifier of the role.</param>
    /// <returns>The <see cref="UserRole"/> link if found.</returns>
    Task<UserRole?> GetUserRoleLink(Guid userId, Guid roleId);
}
