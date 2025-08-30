namespace HealthConnect.Infrastructure.Repositories;

using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Domain.Models;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Repository for managing User entities.
/// </summary>
public class UserRepository(AppDbContext appDbContext) : IUserRepository
{
    private readonly AppDbContext _appDbContext = appDbContext;

    /// <summary>
    /// Asynchronously adds a new user to the database.
    /// </summary>
    /// <param name="User">The User object to be created.</param>
    /// <returns>A Task that represents the asynchronous add operation.</returns>
    public async Task CreateUserAsync(User User)
    {
        await _appDbContext.Users.AddAsync(User);
    }

    /// <summary>
    /// Asynchronously retrieves a collection of all registered users.
    /// </summary>
    /// <returns>
    /// A Task that, upon completion, contains an IEnumerable<User> collection with all users.
    /// </returns>
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _appDbContext.Users.ToListAsync();
    }

    /// <summary>
    /// Asynchronously finds a user by their email address.
    /// </summary>
    /// <param name="Email">The email address of the user to search for.</param>
    /// <returns>
    /// A Task that, upon completion, contains the User object corresponding to the provided email,
    /// or <c>null</c> if no user is found.
    /// </returns>
    public async Task<User?> GetUserByEmailAsync(string Email)
    {
        return await _appDbContext.Users.Include(u => u.Doctor)
            .FirstOrDefaultAsync(u => u.Email == Email);
    }

    /// <summary>
    /// Asynchronously finds a user by their unique identifier (ID).
    /// </summary>
    /// <param name="Id">The Guid of the user to search for.</param>
    /// <returns>
    /// A Task that, upon completion, contains the User object corresponding to the provided ID,
    /// or <c>null</c> if no user is found.
    /// </returns>
    public async Task<User?> GetUserByIdAsync(Guid Id)
    {
        return await _appDbContext.Users.FindAsync(Id);
    }

    public async Task<User?> GetDoctorByEmailAsync(string email)
    {
        return await _appDbContext.Users
            .Include(u => u.Doctor)
            .FirstOrDefaultAsync(u => u.Email == email && u.Doctor != null);
    }
}
