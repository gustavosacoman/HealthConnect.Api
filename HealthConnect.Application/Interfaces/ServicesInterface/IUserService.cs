namespace HealthConnect.Application.Interfaces.ServicesInterface;

using HealthConnect.Application.Dtos.Doctors;
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
    public Task<UserSummaryDto> GetUserByIdAsync(Guid id);

    /// <summary>
    /// Gets a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <returns>The summary information of the user.</returns>
    public Task<UserSummaryDto> GetUserByEmailAsync(string email);

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <returns>A collection of user summary information.</returns>
    public Task<IEnumerable<UserSummaryDto>> GetAllUsersAsync();


    /// <summary>
    /// Creates a new doctor user with the provided registration data.
    /// </summary>
    /// <param name="data">The registration data for the doctor.</param>
    /// <returns>The summary information of the created doctor user.</returns>
    public Task<DoctorDetailDto> CreateDoctorAsync(DoctorRegistrationDto data);

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="Id">The unique identifier of the user to update.</param>
    /// <param name="data">The updated user data.</param>
    /// <returns>The summary information of the updated user.</returns>
    public Task<UserSummaryDto> UpdateUserAsync(Guid Id, UserUpdatingDto data);

    /// <summary>
    /// Deletes a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task DeleteUserAsync(string email);
}
