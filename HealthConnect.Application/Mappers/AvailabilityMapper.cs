using AutoMapper;
using HealthConnect.Application.Dtos.Availability;
using HealthConnect.Application.Dtos.Doctors;
using HealthConnect.Domain.Models;

namespace HealthConnect.Application.Mappers;

public class AvailabilityMapper : Profile
{
     public AvailabilityMapper()
    {
        CreateMap<Availability, AvailabilitySummaryDto>()
            .ForMember(desc => desc.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(desc => desc.DoctorId, opt => opt.MapFrom(src => src.DoctorId))
            .ForMember(desc => desc.SlotDateTime, opt => opt.MapFrom(src => src.SlotDateTime))
            .ForMember(desc => desc.DurationMinutes, opt => opt.MapFrom(src => src.DurationMinutes))
            .ForMember(desc => desc.Name, opt => opt.MapFrom(src => src.Doctor.User.Name))
            .ForMember(dest => dest.Specialities, opt => opt.MapFrom(src => src.Doctor.DoctorSpecialities.Select(ds => new DoctorSpecialityDetailDto
            {
                SpecialityName = ds.Speciality.Name,
                RqeNumber = ds.RqeNumber,
            }).ToList()));

    }
}
