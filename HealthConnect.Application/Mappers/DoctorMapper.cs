namespace HealthConnect.Application.Mappers;

using AutoMapper;
using HealthConnect.Application.Dtos.Doctors;
using HealthConnect.Domain.Models;

public class DoctorMapper : Profile
{
    public DoctorMapper()
    {
        this.CreateMap<Doctor, DoctorSummaryDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.RQE, opt => opt.MapFrom(opt => opt.RQE))
            .ForMember(dest => dest.CRM, opt => opt.MapFrom(opt => opt.DoctorCRMs.Select(crm => crm.CRMNumber).ToList()))
            .ForMember(dest => dest.State, opt => opt.MapFrom(opt => opt.DoctorCRMs.Select(crm => crm.State).ToList()))
            .ForMember(dest => dest.Speciality, opt => opt.MapFrom(src => src.Speciality.Name))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Biography, opt => opt.MapFrom(src => src.Biography))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.User.UserRoles.Select(ur => ur.Role.Name).ToList()));
        this.CreateMap<Doctor, DoctorDetailDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.RQE, opt => opt.MapFrom(opt => opt.RQE))
            .ForMember(dest => dest.CRM, opt => opt.MapFrom(opt => opt.DoctorCRMs.Select(crm => crm.CRMNumber).ToList()))
            .ForMember(dest => dest.State, opt => opt.MapFrom(opt => opt.DoctorCRMs.Select(crm => crm.State).ToList()))
            .ForMember(dest => dest.Speciality, opt => opt .MapFrom(src => src.Speciality.Name))
            .ForMember(dest => dest.Biography, opt => opt.MapFrom(src => src.Biography))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.Phone))
            .ForMember(dest => dest.CPF, opt => opt.MapFrom(src => src.User.CPF))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.User.BirthDate))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.User.UserRoles.Select(ur => ur.Role.Name).ToList()));

    }
}
