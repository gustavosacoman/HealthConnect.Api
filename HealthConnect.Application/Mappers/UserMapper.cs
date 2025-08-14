using AutoMapper;
using HealthConnect.Application.Dtos;
using HealthConnect.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Application.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserSummaryDto>();
    }
}
