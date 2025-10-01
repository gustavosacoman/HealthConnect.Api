namespace HealthConnect.Infrastructure.Repositories;

using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Domain.Models;
using HealthConnect.Domain.Models.Roles;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Repository for managing User entities.
/// </summary>
public class UserRepository(
    AppDbContext appDbContext)
    : IUserRepository
{
    private readonly AppDbContext _appDbContext = appDbContext;

    /// <summary>
    /// Asynchronously adds a new user to the database.
    /// </summary>
    /// <param name="user">The User object to be created.</param>
    /// <returns>A Task that represents the asynchronous add operation.</returns>
    public async Task CreateUserAsync(User user)
    {
        await _appDbContext.Users.AddAsync(user);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _appDbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _appDbContext.Users
            .Include(u => u.Doctor)
            .Include(u => u.Client)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    /// <inheritdoc/>
    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _appDbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    /// <inheritdoc/>
    public async Task<User?> GetDoctorByEmailAsync(string email)
    {
        return await _appDbContext.Users
            .Include(u => u.Doctor)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email && u.Doctor != null);
    }

    /// <inheritdoc/>
    public async Task AddUserRoleLinkAsync(UserRole userRole)
    {
        await _appDbContext.UserRoles.AddAsync(userRole);
    }

    /// <inheritdoc/>
    public void RemoveRoleLinkAsync(UserRole userRole)
    {
        _appDbContext.UserRoles.Remove(userRole);
    }

    /// <inheritdoc/>
    public async Task<UserRole?> GetUserRoleLink(Guid userId, Guid roleId)
    {
        return await _appDbContext.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
    }
}
