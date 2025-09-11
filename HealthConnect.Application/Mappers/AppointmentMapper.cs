using AutoMapper;
using HealthConnect.Application.Dtos.Appointment;
using HealthConnect.Domain.Models;

namespace HealthConnect.Application.Mappers;

public class AppointmentMapper : Profile
{
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
            .ForMember(desc => desc.Status, opt => opt.MapFrom(src => src.AppointmentStatus.ToString()))
            .ForMember(desc => desc.Notes, opt => opt.MapFrom(src => src.Notes));
    }
}
