namespace HealthConnect.Application.Mappers;

using AutoMapper;
using HealthConnect.Application.Dtos.Client;
using HealthConnect.Domain.Models;

/// <summary>
/// Provides AutoMapper profiles for mapping <see cref="Client"/> domain models to DTOs.
/// </summary>
public class ClientMapper : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientMapper"/> class and configures mapping profiles.
    /// </summary>
    public ClientMapper()
    {
        CreateMap<Client, ClientSummaryDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => src.User.Sex.ToString()));

        CreateMap<Client, ClientDetailDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.CPF, opt => opt.MapFrom(src => src.User.CPF))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.User.BirthDate))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.Phone))
            .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => src.User.Sex.ToString()));
    }
}
