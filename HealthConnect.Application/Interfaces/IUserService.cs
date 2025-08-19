namespace HealthConnect.Application.Interfaces;

using HealthConnect.Application.Dtos.Users;

/// <summary>
/// Provides user-related operations such as retrieval, creation, updating, and deletion.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>The summary information of the user.</returns>
    public Task<UserSummaryDto> GetUserById(Guid id);

    /// <summary>
    /// Gets a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <returns>The summary information of the user.</returns>
    public Task<UserSummaryDto> GetUserByEmail(string email);

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <returns>A collection of user summary information.</returns>
    public Task<IEnumerable<UserSummaryDto>> GetAllUsers();

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="data">The registration data for the new user.</param>
    /// <returns>The summary information of the created user.</returns>
    public Task<UserSummaryDto> CreateUser(UserRegistrationDto data);

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="Id">The unique identifier of the user to update.</param>
    /// <param name="data">The updated user data.</param>
    /// <returns>The summary information of the updated user.</returns>
    public Task<UserSummaryDto> UpdateUser(Guid Id, UserUpdatingDto data);

    /// <summary>
    /// Deletes a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task DeleteUser(string email);
}
