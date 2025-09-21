using HealthConnect.Domain.Models;

namespace HealthConnect.Application.Interfaces.RepositoriesInterfaces;

public interface IClientRepository
{
    public Task<Client> GetClientByIdAsync(Guid Id);

    public Task<Client> GetClientByUserIdAsync(Guid userId);

    public IQueryable<Client> GetAllClientsAsync();

    public Task CreateClientAsync(Client client);
}
