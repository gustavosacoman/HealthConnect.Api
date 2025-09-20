namespace HealthConnect.Application.Interfaces.ServicesInterface;

using HealthConnect.Application.Dtos.Role;

public interface IRoleService
{
    Task<RoleSummaryDto> GetRoleByNameAsync(string roleName);

    Task<IEnumerable<RoleSummaryDto>> GetAllRolesAsync();

    Task<RoleSummaryDto> GetRoleByIdAsync(Guid roleId);

    Task<IEnumerable<RoleSummaryDto>> GetRolesForUserAsync(Guid userId);

    Task CreateRoleAsync(RoleRegistrationDto roleRegistration);
}
