namespace HealthConnect.Application.Mappers;

using AutoMapper;
using HealthConnect.Application.Dtos.Appointment;
using HealthConnect.Domain.Models;

/// <summary>
/// AutoMapper profile for mapping <see cref="Appointment"/> domain models to <see cref="AppointmentDetailDto"/> DTOs.
/// </summary>
public class AppointmentMapper : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppointmentMapper"/> class and configures mapping rules.
    /// </summary>
    public AppointmentMapper()
    {
        CreateMap<Appointment, AppointmentDetailDto>()
            .ForMember(desc => desc.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(desc => desc.AvailabilityId, opt => opt.MapFrom(src => src.AvailabilityId))
            .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.ClientId))
            .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId))
            .ForMember(desc => desc.AppointmentDate, opt => opt.MapFrom(src => src.Availability.SlotDateTime))
            .ForMember(desc => desc.ClientName, opt => opt.MapFrom(src => src.Client.User.Name))
            .ForMember(desc => desc.DoctorName, opt => opt.MapFrom(src => src.Doctor.User.Name))
            .ForMember(desc => desc.Duration, opt => opt.MapFrom(src => src.Availability.DurationMinutes))
            .ForMember(desc => desc.Status, opt => opt.MapFrom(src => src.AppointmentStatus.ToString()))
            .ForMember(desc => desc.Notes, opt => opt.MapFrom(src => src.Notes));
    }
}
