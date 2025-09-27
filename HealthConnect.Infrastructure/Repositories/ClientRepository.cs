namespace HealthConnect.Infrastructure.Repositories;

using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Domain.Models;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Repository for managing patients.
/// </summary>
public class ClientRepository(AppDbContext appDbContext) : IClientRepository
{
    private readonly AppDbContext _appDbContex = appDbContext;

    /// <inheritdoc/>
    public async Task CreateClientAsync(Client client)
    {
        await _appDbContex.Clients.AddAsync(client);
    }

    /// <inheritdoc/>
    public IQueryable<Client> GetAllClientsAsync()
    {
        return _appDbContex.Clients.AsNoTracking();
    }

    /// <inheritdoc/>
    public async Task<Client?> GetClientByIdAsync(Guid Id)
    {
        return await _appDbContex.Clients
            .Include(c => c.User)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(c => c.Id == Id);
    }

    /// <inheritdoc/>
    public async Task<Client?> GetClientByUserIdAsync(Guid userId)
    {
        return await _appDbContex.Clients
            .Include(c => c.User)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }
}
