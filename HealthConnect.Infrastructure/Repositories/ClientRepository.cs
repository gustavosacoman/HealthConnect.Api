using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Domain.Models;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthConnect.Infrastructure.Repositories;
public class ClientRepository(AppDbContext appDbContext) : IClientRepository
{
    private readonly AppDbContext _appDbContex = appDbContext;

    public async Task CreateClientAsync(Client client)
    {
        await _appDbContex.Clients.AddAsync(client);
    }

    public async Task<IEnumerable<Client>> GetAllClientsAsync()
    {
        return await _appDbContex.Clients.Include(c => c.User).ToListAsync();
    }

    public async Task<Client> GetClientByIdAsync(Guid Id)
    {
        return await _appDbContex.Clients
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == Id);
    }

    public async Task<Client> GetClientByUserIdAsync(Guid userId)
    {
        return await _appDbContex.Clients
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }
}
