namespace HealthConnect.Infrastructure.Repositories;

using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Domain.Models.Roles;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Repository for managing roles and user-role assignments.
/// </summary>
public class RoleRepository(
    AppDbContext appDbContext)
    : IRoleRepository
{
    private readonly AppDbContext _appDbContext = appDbContext;

    /// <inheritdoc/>
    public async Task CreateRoleAsync(Role role)
    {
        await _appDbContext.Roles.AddAsync(role);
    }

    /// <inheritdoc/>
    public async Task CreateUserRoleAsync(UserRole userRole)
    {
        await _appDbContext.UserRoles.AddAsync(userRole);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Role>> GetAllRolesAsync()
    {
        return await _appDbContext.Roles.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Role>> GetRolesForUserAsync(Guid userId)
    {
        return await _appDbContext.Roles
            .Where(r =>
            r.UserRoles
            .Any(ur => ur.UserId == userId))
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Role?> GetRoleByIdAsync(Guid roleId)
    {
        return await _appDbContext.Roles.FindAsync(roleId);
    }

    /// <inheritdoc/>
    public async Task<Role?> GetRoleByNameAsync(string roleName)
    {
        return await _appDbContext.Roles.FirstOrDefaultAsync(r => r.Name.ToLower() == roleName.ToLower());
    }
}
