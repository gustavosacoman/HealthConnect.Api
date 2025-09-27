using AutoMapper;
using HealthConnect.Application.Dtos.Speciality;
using HealthConnect.Domain.Models.Specialities;

namespace HealthConnect.Application.Mappers;

public class SpecialityMapper : Profile
{
    public SpecialityMapper()
    {
        CreateMap<Speciality, SpecialitySummaryDto>();
    }
}
