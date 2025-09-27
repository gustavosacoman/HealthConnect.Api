namespace HealthConnect.Application.Mappers;

using AutoMapper;
using HealthConnect.Application.Dtos.DoctorCRM;
using HealthConnect.Domain.Models;

/// <summary>
/// AutoMapper profile for mapping <see cref="DoctorCRM"/> to <see cref="DoctorCRMSummaryDto"/>.
/// </summary>
public class DoctorCRMMapper : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DoctorCRMMapper"/> class.
    /// Configures mapping from <see cref="DoctorCRM"/> to <see cref="DoctorCRMSummaryDto"/>.
    /// </summary>
    public DoctorCRMMapper()
    {
        CreateMap<DoctorCRM, DoctorCRMSummaryDto>()
            .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.User.Name));
    }
}
