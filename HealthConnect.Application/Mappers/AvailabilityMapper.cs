namespace HealthConnect.Application.Mappers;

using AutoMapper;
using HealthConnect.Application.Dtos.Availability;
using HealthConnect.Application.Dtos.Doctors;
using HealthConnect.Domain.Models;

/// <summary>
/// Provides AutoMapper profile configuration for mapping <see cref="Availability"/> domain models
/// to <see cref="AvailabilitySummaryDto"/> DTOs.
/// </summary>
public class AvailabilityMapper : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AvailabilityMapper"/> class.
    /// Configures mapping from <see cref="Availability"/> to <see cref="AvailabilitySummaryDto"/>.
    /// </summary>
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
