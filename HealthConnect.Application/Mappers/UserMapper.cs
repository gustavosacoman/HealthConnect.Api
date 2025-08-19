namespace HealthConnect.Application.Mappers;

using AutoMapper;
using HealthConnect.Application.Dtos.Users;
using HealthConnect.Domain.Models;

/// <summary>
/// AutoMapper profile for mapping <see cref="User"/> domain models to <see cref="UserSummaryDto"/> DTOs.
/// </summary>
public class UserMapper : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserMapper"/> class and configures mapping rules.
    /// </summary>
    public UserMapper()
    {
        this.CreateMap<User, UserSummaryDto>();
    }
}
