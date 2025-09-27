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
            .ForMember(dest => dest.Specialities, opt => opt.MapFrom(src => src.DoctorSpecialities.Select(ds => new DoctorSpecialityDetailDto
            {
                SpecialityName = ds.Speciality.Name,
                RqeNumber = ds.RqeNumber,
            }).ToList()))
            .ForMember(dest => dest.CRMs, opt => opt.MapFrom(src => src.DoctorCRMs.Select(crm => new DoctorCrmDetailDto
            {
                CrmNumber = crm.CRMNumber,
                State = crm.State,
            }).ToList()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Biography, opt => opt.MapFrom(src => src.Biography));
        this.CreateMap<Doctor, DoctorDetailDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Specialities, opt => opt.MapFrom(src => src.DoctorSpecialities.Select(ds => new DoctorSpecialityDetailDto
            {
                SpecialityName = ds.Speciality.Name,
                RqeNumber = ds.RqeNumber,
            }).ToList()))
            .ForMember(dest => dest.CRMs, opt => opt.MapFrom(src => src.DoctorCRMs.Select(crm => new DoctorCrmDetailDto
            {
                CrmNumber = crm.CRMNumber,
                State = crm.State,
            }).ToList()))
            .ForMember(dest => dest.Biography, opt => opt.MapFrom(src => src.Biography))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.Phone))
            .ForMember(dest => dest.CPF, opt => opt.MapFrom(src => src.User.CPF))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.User.BirthDate));
    }
}
