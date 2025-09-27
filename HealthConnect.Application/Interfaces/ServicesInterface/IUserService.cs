namespace HealthConnect.Application.Interfaces.ServicesInterface;

using HealthConnect.Application.Dtos.Client;
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
    Task<UserSummaryDto> GetUserByIdAsync(Guid id);

    /// <summary>
    /// Gets a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <returns>The summary information of the user.</returns>
    Task<UserSummaryDto> GetUserByEmailAsync(string email);

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <returns>A collection of user summary information.</returns>
    Task<IEnumerable<UserSummaryDto>> GetAllUsersAsync();

    /// <summary>
    /// Creates a new doctor user with the provided registration data.
    /// </summary>
    /// <param name="data">The registration data for the doctor.</param>
    /// <returns>The summary information of the created doctor user.</returns>
    Task<DoctorDetailDto> CreateDoctorAsync(DoctorRegistrationDto data);

    /// <summary>
    /// Creates a new client user with the provided registration data.
    /// </summary>
    /// <param name="data">The registration data for the client.</param>
    /// <returns>The detail information of the created client user.</returns>
    Task<ClientDetailDto> CreateClientAsync(ClientRegistrationDto data);

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="id">The unique identifier of the user to update.</param>
    /// <param name="data">The updated user data.</param>
    /// <returns>The summary information of the updated user.</returns>
    Task<UserSummaryDto> UpdateUserAsync(Guid id, UserUpdatingDto data);

    /// <summary>
    /// Deletes a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteUserAsync(string email);

    /// <summary>
    /// Adds a role link to a user.
    /// </summary>
    /// <param name="userRoleRequestDto">The request containing the user's email and the role name to link.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddRoleLinkToUserAsync(UserRoleRequestDto userRoleRequestDto);

    /// <summary>
    /// Removes a role link from a user.
    /// </summary>
    /// <param name="userRoleRequestDto">The request containing the user's email and the role name to unlink.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RemoveRoleLinkFromUserAsync(UserRoleRequestDto userRoleRequestDto);

    /// <summary>
    /// Gets a doctor user by their email address.
    /// </summary>
    /// <param name="email">The email address of the doctor user.</param>
    /// <returns>The detail information of the doctor user.</returns>
    public Task<DoctorDetailDto> GetDoctorByEmailAsync(string email);
}
