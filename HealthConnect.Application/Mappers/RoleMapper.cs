using AutoMapper;
using HealthConnect.Application.Dtos.Role;
using HealthConnect.Domain.Models.Roles;

namespace HealthConnect.Application.Mappers;

public class RoleMapper : Profile
{
    public RoleMapper()
    {
        CreateMap<Role, RoleSummaryDto>();
    }
}
