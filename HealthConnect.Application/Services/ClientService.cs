using AutoMapper;
using AutoMapper.QueryableExtensions;
using HealthConnect.Application.Dtos.Client;
using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Application.Interfaces.ServicesInterface;
using Microsoft.EntityFrameworkCore;

namespace HealthConnect.Application.Services;
public class ClientService(IClientRepository clientRepository, IMapper mapper ) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<ClientSummaryDto>> GetAllClientsAsync()
    {
        var queriable = _clientRepository.GetAllClientsAsync();

        var clients = queriable.OrderBy(c => c.User.Name)
            .ProjectTo<ClientSummaryDto>(_mapper.ConfigurationProvider);

        return await clients.ToListAsync();
    }

    public async Task<ClientSummaryDto> GetClientByIdAsync(Guid clientId)
    {
        if (clientId == Guid.Empty)
        {
            throw new ArgumentException("Client ID cannot be empty.", nameof(clientId));
        }

        var user = await _clientRepository.GetClientByIdAsync(clientId) ??
            throw new KeyNotFoundException($"Client with ID {clientId} not found.");

        return _mapper.Map<ClientSummaryDto>(user);
    }

    public async Task<ClientSummaryDto> GetClientByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new KeyNotFoundException("User ID cannot be empty.");
        }

        var user = await _clientRepository.GetClientByUserIdAsync(userId) ??
            throw new KeyNotFoundException($"Client with User ID {userId} not found.");

        return _mapper.Map<ClientSummaryDto>(user);
    }

    public async Task<ClientDetailDto> GetClientDetailByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new NullReferenceException("Client ID cannot be empty.");
        }

        var user = await _clientRepository.GetClientByIdAsync(id) ??
             throw new KeyNotFoundException($"Client with ID {id} not found.");

        return _mapper.Map<ClientDetailDto>(user);
    }
}
