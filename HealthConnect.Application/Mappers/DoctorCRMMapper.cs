using AutoMapper;
using HealthConnect.Application.Dtos.DoctorCRM;
using HealthConnect.Domain.Models;

namespace HealthConnect.Application.Mappers;

public class DoctorCRMMapper : Profile
{
    public DoctorCRMMapper()
    {
        CreateMap<DoctorCRM, DoctorCRMSummaryDto>()
            .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.User.Name));
    }
}
