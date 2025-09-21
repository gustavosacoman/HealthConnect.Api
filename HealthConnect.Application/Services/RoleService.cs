namespace HealthConnect.Application.Services;

using AutoMapper;
using HealthConnect.Application.Dtos.Role;
using HealthConnect.Application.Interfaces;
using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Models.Roles;

public class RoleService(
    IRoleRepository roleRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IRoleService
{
    private readonly IRoleRepository _roleRepository = roleRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<RoleSummaryDto> GetRoleByNameAsync(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            throw new ArgumentException("Role name cannot be null or empty.", nameof(roleName));
        }

        var role =  await _roleRepository.GetRoleByNameAsync(roleName.ToLower()) ?? 
                throw new KeyNotFoundException($"Role with name '{roleName}' not found.");

        return _mapper.Map<RoleSummaryDto>(role);
    }

    public async Task<IEnumerable<RoleSummaryDto>> GetAllRolesAsync()
    {
        var roles = await _roleRepository.GetAllRolesAsync();
        return _mapper.Map<IEnumerable<RoleSummaryDto>>(roles);
    }

    public async Task<RoleSummaryDto> GetRoleByIdAsync(Guid roleId)
    {
        if (roleId == Guid.Empty)
        {
            throw new ArgumentException("Role ID cannot be empty.", nameof(roleId));
        }

        var role = await _roleRepository.GetRoleByIdAsync(roleId) ?? 
                throw new KeyNotFoundException($"Role with ID '{roleId}' not found.");

        return _mapper.Map<RoleSummaryDto>(role);
    }

    public async Task<IEnumerable<RoleSummaryDto>> GetRolesForUserAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));
        }

        var roles = await _roleRepository.GetRolesForUserAsync(userId) ?? 
                throw new KeyNotFoundException($"No roles found for user with ID '{userId}'.");
        return _mapper.Map<IEnumerable<RoleSummaryDto>>(roles);
    }

    public async Task CreateRoleAsync(RoleRegistrationDto roleRegistration)
    {
        if (string.IsNullOrWhiteSpace(roleRegistration.Name))
        {
            throw new ArgumentException("Role name cannot be null or empty.", nameof(roleRegistration.Name));
        }

        var existingRole = await _roleRepository.GetRoleByNameAsync(roleRegistration.Name.ToLower());
        if (existingRole != null)
        {
            throw new InvalidOperationException($"Role with name '{roleRegistration.Name}' already exists.");
        }

        var newRole = new Role
        {
            Id = Guid.NewGuid(),
            Name = roleRegistration.Name.ToLower(),
        };

        await _roleRepository.CreateRoleAsync(newRole);
        await _unitOfWork.SaveChangesAsync();
    }
}
