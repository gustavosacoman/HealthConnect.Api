namespace HealthConnect.Application.Mappers;

using AutoMapper;
using HealthConnect.Application.Dtos.Role;
using HealthConnect.Domain.Models.Roles;

/// <summary>
/// AutoMapper profile for mapping <see cref="Role"/> domain models to <see cref="RoleSummaryDto"/> DTOs.
/// </summary>
public class RoleMapper : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RoleMapper"/> class and configures mapping rules.
    /// </summary>
    public RoleMapper()
    {
        CreateMap<Role, RoleSummaryDto>();
    }
}
