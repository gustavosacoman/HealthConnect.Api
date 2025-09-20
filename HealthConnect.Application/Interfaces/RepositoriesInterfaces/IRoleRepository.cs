using HealthConnect.Domain.Models.Roles;

namespace HealthConnect.Application.Interfaces.RepositoriesInterfaces;

public interface IRoleRepository
{
    Task<IEnumerable<Role>> GetRolesForUserAsync(Guid userId);

    Task<IEnumerable<Role>> GetAllRolesAsync();

    Task<Role?> GetRoleByNameAsync(string roleName);

    Task<Role?> GetRoleByIdAsync(Guid roleId);

    Task CreateRoleAsync(Role role);

    Task CreateUserRoleAsync(UserRole userRole);
}
