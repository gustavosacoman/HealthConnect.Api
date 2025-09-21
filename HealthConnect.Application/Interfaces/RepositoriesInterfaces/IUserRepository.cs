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
    /// <param name="Id">The unique identifier of the user.</param>
    /// <returns>The <see cref="User"/> entity if found; otherwise, <c>null</c>.</returns>
    Task<User?> GetUserByIdAsync(Guid Id);

    /// <summary>
    /// Retrieves a user by their email address.
    /// </summary>
    /// <param name="Email">The email address of the user.</param>
    /// <returns>The <see cref="User"/> entity if found; otherwise, <c>null</c>.</returns>
    Task<User?> GetUserByEmailAsync(string Email);

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="User">The <see cref="User"/> entity to create.</param>
    Task CreateUserAsync(User User);

    Task<User?> GetDoctorByEmailAsync(string email);

    Task RemoveRoleLinkAsync(UserRole userRole);

    Task AddUserRoleLinkAsync(UserRole userRole);

    Task<UserRole> GetUserRoleLink(Guid userId, Guid roleId);
}
