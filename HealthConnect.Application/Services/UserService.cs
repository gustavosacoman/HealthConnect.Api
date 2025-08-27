namespace HealthConnect.Application.Services;

using AutoMapper;
using HealthConnect.Application.Dtos.Users;
using HealthConnect.Application.Interfaces;
using HealthConnect.Domain.Models;

/// <summary>
/// Provides handling of user business rules for retrieval, creation, update, and deletion.
/// </summary>
public class UserService(
    IUserRepository userRepository,
    IMapper mapper,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    /// <summary>
    /// Gets a user by their unique identifier.
    /// </summary>
    /// <param name="Id">The user ID.</param>
    /// <returns>The user summary DTO.</returns>
    public async Task<UserSummaryDto> GetUserById(Guid Id)
    {
        if (Id == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(Id));
        }

        var user = await _userRepository.GetUserById(Id);

        return _mapper.Map<UserSummaryDto>(user)
            ?? throw new KeyNotFoundException($"User with ID {Id} not found.");
    }

    /// <summary>
    /// Gets a user by their email address.
    /// </summary>
    /// <param name="Email">The user's email.</param>
    /// <returns>The user summary DTO.</returns>
    public async Task<UserSummaryDto> GetUserByEmail(string Email)
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            throw new ArgumentException("Email cannot be null or empty.", nameof(Email));
        }

        var user = await _userRepository.GetUserByEmail(Email);

        return _mapper.Map<UserSummaryDto>(user)
            ?? throw new KeyNotFoundException($"User with email {Email} not found.");
    }

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <returns>A collection of user summary DTOs.</returns>
    public async Task<IEnumerable<UserSummaryDto>> GetAllUsers()
    {
        var users = await _userRepository.GetAllUsers();
        return _mapper.Map<IEnumerable<UserSummaryDto>>(users);
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="data">The user registration data.</param>
    /// <returns>The created user summary DTO.</returns>
    public async Task<UserSummaryDto> CreateUser(UserRegistrationDto data)
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data), "User cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(data.Name) || string.IsNullOrWhiteSpace(data.Email) ||
            string.IsNullOrWhiteSpace(data.Password) || string.IsNullOrWhiteSpace(data.CPF))
        {
            throw new ArgumentException("User data is incomplete.");
        }

        if (await _userRepository.GetUserByEmail(data.Email) != null)
        {
            throw new InvalidOperationException($"User with email {data.Email} already exists.");
        }

        var salt = _passwordHasher.GenerateSalt();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = data.Name,
            Email = data.Email,
            CPF = data.CPF,
            Salt = salt,
            HashedPassword = _passwordHasher.HashPassword(data.Password, salt),
            BirthDate = data.BirthDate,
        };

        await _userRepository.CreateUser(user);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<UserSummaryDto>(user);
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="Id">The user ID.</param>
    /// <param name="data">The user update data.</param>
    /// <returns>The updated user summary DTO.</returns>
    public async Task<UserSummaryDto> UpdateUser(Guid Id, UserUpdatingDto data)
    {
        if (Id == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(Id));
        }

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data), "User data cannot be null.");
        }

        var user = await _userRepository.GetUserById(Id)
            ?? throw new KeyNotFoundException($"User with ID {Id} not found.");

        user.Name = data.Name ?? user.Name;
        user.Phone = data.Phone ?? user.Phone;

        if (!string.IsNullOrWhiteSpace(data.Password))
        {
            var salt = _passwordHasher.GenerateSalt();
            user.Salt = salt;
            user.HashedPassword = _passwordHasher.HashPassword(data.Password, salt);
        }

        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<UserSummaryDto>(user);
    }

    /// <summary>
    /// Deletes a user by their email address.
    /// </summary>
    /// <param name="email">The user's email.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task DeleteUser(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(email));
        }

        var user = await _userRepository.GetUserByEmail(email) ??
            throw new KeyNotFoundException($"User with ID {email} not found.");

        user.DeletedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync();
    }
}
