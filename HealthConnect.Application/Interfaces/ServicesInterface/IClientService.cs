using HealthConnect.Application.Dtos.Client;

namespace HealthConnect.Application.Interfaces.ServicesInterface;

public interface IClientService
{
    public Task<ClientDetailDto> GetClientDetailByIdAsync(Guid userId);

    public Task<ClientSummaryDto> GetClientByIdAsync(Guid clientId);

    public Task<ClientDetailDto> GetClientByUserIdAsync(Guid userId);

    public Task<IEnumerable<ClientSummaryDto>> GetAllClientsAsync();
}
