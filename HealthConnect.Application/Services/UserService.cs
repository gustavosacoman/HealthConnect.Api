namespace HealthConnect.Application.Services;

using AutoMapper;
using HealthConnect.Application.Dtos.Client;
using HealthConnect.Application.Dtos.Doctors;
using HealthConnect.Application.Dtos.Users;
using HealthConnect.Application.Interfaces;
using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Models;
using HealthConnect.Domain.Models.Roles;
using HealthConnect.Domain.Models.Specialities;

/// <summary>
/// Provides handling of user business rules for retrieval, creation, update, and deletion.
/// </summary>
public class UserService(
    IUserRepository userRepository,
    IMapper mapper,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork,
    IDoctorRepository doctorRepository,
    IClientRepository clientRepository,
    ISpecialityRepository specialityRepository,
    IRoleRepository roleRepository,
    IDoctorCRMRepository doctorCRMRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDoctorRepository _doctorRepository = doctorRepository;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly ISpecialityRepository _specialityRepository = specialityRepository;
    private readonly IRoleRepository _roleRepository = roleRepository;
    private readonly IDoctorCRMRepository _doctorCRMRepository = doctorCRMRepository;

    /// <summary>
    /// Gets a user by their unique identifier.
    /// </summary>
    /// <param name="Id">The user ID.</param>
    /// <returns>The user summary DTO.</returns>
    public async Task<UserSummaryDto> GetUserByIdAsync(Guid Id)
    {
        if (Id == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(Id));
        }

        var user = await _userRepository.GetUserByIdAsync(Id);

        return _mapper.Map<UserSummaryDto>(user)
            ?? throw new KeyNotFoundException($"User with ID {Id} not found.");
    }

    /// <summary>
    /// Gets a user by their email address.
    /// </summary>
    /// <param name="Email">The user's email.</param>
    /// <returns>The user summary DTO.</returns>
    public async Task<UserSummaryDto> GetUserByEmailAsync(string Email)
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            throw new ArgumentException("Email cannot be null or empty.", nameof(Email));
        }

        var user = await _userRepository.GetUserByEmailAsync(Email);

        return _mapper.Map<UserSummaryDto>(user)
            ?? throw new KeyNotFoundException($"User with email {Email} not found.");
    }

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <returns>A collection of user summary DTOs.</returns>
    public async Task<IEnumerable<UserSummaryDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return _mapper.Map<IEnumerable<UserSummaryDto>>(users);
    }

    public async Task<DoctorDetailDto> GetDoctorByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));
        }

        var user = await _userRepository.GetDoctorByEmailAsync(email);

        return _mapper.Map<DoctorDetailDto>(user.Doctor)
            ?? throw new KeyNotFoundException($"No doctor profile found for user with email {email}.");
    }
    /// <summary>
    /// Creates a new doctor user in the system.
    /// </summary>
    /// <param name="data">The doctor registration data.</param>
    /// <returns>The summary DTO of the created user.</returns>
    public async Task<DoctorDetailDto> CreateDoctorAsync(DoctorRegistrationDto data)
    {
        if (await _userRepository.GetUserByEmailAsync(data.Email) != null)
        {
            throw new InvalidOperationException($"User with email {data.Email} already exists.");
        }

        if (await _doctorRepository.GetDoctorByRQE(data.RQE) != null)
        {
            throw new InvalidOperationException($"Doctor with RQE {data.RQE} already exists.");
        }

        var speciality = await _specialityRepository.GetSpecialityByNameAsync(data.Speciality);
        if (speciality == null)
        {
            throw new KeyNotFoundException($"Speciality with name {data.Speciality} not found.");
        }

        var doctorRole = await _roleRepository.GetRoleByNameAsync("Doctor") ?? 
             throw new KeyNotFoundException("Role 'Doctor' not found in the system.");

        var salt = _passwordHasher.GenerateSalt();
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = data.Name,
            Email = data.Email,
            CPF = data.CPF,
            Phone = data.Phone,
            Salt = salt,
            HashedPassword = _passwordHasher.HashPassword(data.Password, salt),
            BirthDate = data.BirthDate,
            UserRoles = new List<UserRole>(),
            Sex = data.Sex,
        };
        var doctor = new Doctor
        {
            Id = Guid.NewGuid(),
            User = user,
            UserId = user.Id,
            RQE = data.RQE,
            Biography = data.Biography,
        };

        var existRqe = await _specialityRepository.GetDoctorSpecialityByRqe(data.RQE);

        if (existRqe != null)
        {
            throw new InvalidOperationException($"Doctor with RQE {data.RQE} is already linked to a speciality.");
        }

        var doctorSpeciality = new DoctorSpeciality
        {
            Doctor = doctor,
            Speciality = speciality,
            RqeNumber = data.RQE,
        };

        var existCRM = await _doctorCRMRepository.GetCRMByCodeAndState(data.CRM, data.CRMState);

        if (existCRM != null)
        {
            throw new InvalidOperationException($"CRM {data.CRM} for state {data.CRMState} is already registered.");
        }

        var newCRM = new DoctorCRM
        {
            Id = Guid.NewGuid(),
            CRMNumber = data.CRM,
            State = data.CRMState,
            Doctor = doctor,
            DoctorId = doctor.Id,
        };

        var newUserRole = new UserRole
        {
            User = user,
            Role = doctorRole,
        };

        await _roleRepository.CreateUserRoleAsync(newUserRole);
        await _userRepository.CreateUserAsync(user);
        await _doctorRepository.CreateDoctor(doctor);
        await _doctorCRMRepository.CreateCRMAsync(newCRM);
        await _doctorRepository.AddDoctorLinkToSpeciality(doctorSpeciality);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<DoctorDetailDto>(doctor);
    }

    public async Task<ClientDetailDto> CreateClientAsync(ClientRegistrationDto data)
    {
        if (await _userRepository.GetUserByEmailAsync(data.Email) != null)
        {
            throw new InvalidOperationException($"User with email {data.Email} already exists.");
        }

        var patientRole = await _roleRepository.GetRoleByNameAsync("Patient") ??
             throw new KeyNotFoundException("Role 'Client' not found in the system.");

        var salt = _passwordHasher.GenerateSalt();
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = data.Name,
            Email = data.Email,
            CPF = data.CPF,
            Phone = data.Phone,
            Salt = salt,
            HashedPassword = _passwordHasher.HashPassword(data.Password, salt),
            BirthDate = data.BirthDate,
            UserRoles = new List<UserRole>(),
            Sex = data.Sex,
        };
        var client = new Client
        {
            Id = Guid.NewGuid(),
            User = user,
            UserId = user.Id,
        };
        user.Client = client;

        var newUserRole = new UserRole
        {
            User = user,
            Role = patientRole,
        };

        await _roleRepository.CreateUserRoleAsync(newUserRole);
        await _userRepository.CreateUserAsync(user);
        await _clientRepository.CreateClientAsync(client);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<ClientDetailDto>(client);
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="Id">The user ID.</param>AutoMapperMappingException: Missing type map configuration 
    /// <param name="data">The user update data.</param>
    /// <returns>The updated user summary DTO.</returns>
    public async Task<UserSummaryDto> UpdateUserAsync(Guid Id, UserUpdatingDto data)
    {
        if (Id == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(Id));
        }

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data), "User data cannot be null.");
        }

        var user = await _userRepository.GetUserByIdAsync(Id)
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
    public async Task DeleteUserAsync(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(email));
        }

        var user = await _userRepository.GetUserByEmailAsync(email) ??
            throw new KeyNotFoundException($"User with ID {email} not found.");

        user.DeletedAt = DateTime.UtcNow;

        if (user.Client != null)
        {
            user.Client.DeletedAt = DateTime.UtcNow;
        }

        if (user.Doctor != null)
        {
            user.Doctor.DeletedAt = DateTime.UtcNow;
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AddRoleLinkToUserAsync(UserRoleRequestDto userRoleRequestDto)
    {
        var user = await _userRepository.GetUserByEmailAsync(userRoleRequestDto.Email)
            ?? throw new KeyNotFoundException($"User with ID {userRoleRequestDto.Email} not found.");

        var role = await _roleRepository.GetRoleByNameAsync(userRoleRequestDto.RoleName.ToLower())
            ?? throw new KeyNotFoundException($"Role with name {userRoleRequestDto.RoleName} not found.");

        var rolesExistInUser = await _roleRepository.GetRolesForUserAsync(user.Id);

        if (rolesExistInUser.Any(r => r.Name == role.Name.ToLower()))
        {
            throw new InvalidOperationException($"User with email {user.Email} already has the role {role.Name}.");
        }

        if ((rolesExistInUser.Any(r => r.Name.ToLower() == "patient") && role.Name.ToLower() == "doctor") ||
            (rolesExistInUser.Any(r => r.Name.ToLower() == "doctor" && role.Name.ToLower() == "patient")))
        {
            throw new InvalidOperationException($"User with email {user.Email} cannot have both Patient and Doctor roles.");
        }

        var userRoleLink = new UserRole
        {
            User = user,
            Role = role,
        };

        await _userRepository.AddUserRoleLinkAsync(userRoleLink);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveRoleLinkFromUserAsync(UserRoleRequestDto userRoleRequestDto)
    {
        var user = await _userRepository.GetUserByEmailAsync(userRoleRequestDto.Email)
            ?? throw new KeyNotFoundException($"User with email {userRoleRequestDto.Email} not found.");

        var role = await _roleRepository.GetRoleByNameAsync(userRoleRequestDto.RoleName.ToLower())
            ?? throw new KeyNotFoundException($"Role with name {userRoleRequestDto.RoleName} not found.");

        var rolesExistInUser = await _roleRepository.GetRolesForUserAsync(user.Id);

        if (rolesExistInUser.Count() == 1)
        {
            throw new InvalidOperationException($"User with email {user.Email} must have at least one role.");
        }

        var userRoleLink = await _userRepository.GetUserRoleLink(user.Id, role.Id)
            ?? throw new KeyNotFoundException($"Role link for user ID {user.Id} and role {role.Name} not found.");

        await _userRepository.RemoveRoleLinkAsync(userRoleLink);
        await _unitOfWork.SaveChangesAsync();
    }
}
