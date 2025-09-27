namespace HealthConnect.Application.Interfaces.ServicesInterface;

using HealthConnect.Application.Dtos.Client;

/// <summary>
/// Provides methods for retrieving client information.
/// </summary>
public interface IClientService
{
    /// <summary>
    /// Gets detailed information about a client by the user's unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A <see cref="ClientDetailDto"/> containing detailed client information.</returns>
    public Task<ClientDetailDto> GetClientDetailByIdAsync(Guid userId);

    /// <summary>
    /// Gets summary information about a client by the client's unique identifier.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client.</param>
    /// <returns>A <see cref="ClientSummaryDto"/> containing summary client information.</returns>
    public Task<ClientSummaryDto> GetClientByIdAsync(Guid clientId);

    /// <summary>
    /// Gets detailed information about a client by the user's unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A <see cref="ClientDetailDto"/> containing detailed client information.</returns>
    public Task<ClientDetailDto> GetClientByUserIdAsync(Guid userId);

    /// <summary>
    /// Gets summary information for all clients.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="ClientSummaryDto"/> objects.</returns>
    public Task<IEnumerable<ClientSummaryDto>> GetAllClientsAsync();
}
