namespace HealthConnect.Application.Interfaces.RepositoriesInterfaces;

using HealthConnect.Domain.Models;

/// <summary>
/// Provides methods for accessing and managing <see cref="Client"/> entities in the data store.
/// </summary>
public interface IClientRepository
{
    /// <summary>
    /// Retrieves a <see cref="Client"/> by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the client.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the client if found; otherwise, null.</returns>
    public Task<Client?> GetClientByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a <see cref="Client"/> by the associated user identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the client if found; otherwise, null.</returns>
    public Task<Client?> GetClientByUserIdAsync(Guid userId);

    /// <summary>
    /// Gets a queryable collection of all <see cref="Client"/> entities.
    /// </summary>
    /// <returns>An <see cref="IQueryable{Client}"/> representing all clients.</returns>
    public IQueryable<Client> GetAllClientsAsync();

    /// <summary>
    /// Creates a new <see cref="Client"/> in the data store.
    /// </summary>
    /// <param name="client">The client entity to create.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task CreateClientAsync(Client client);
}
