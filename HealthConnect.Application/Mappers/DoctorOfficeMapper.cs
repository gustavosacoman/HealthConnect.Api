namespace HealthConnect.Application.Mappers;

using AutoMapper;
using HealthConnect.Application.Dtos.DoctorOffice;
using HealthConnect.Domain.Models;

/// <summary>
/// AutoMapper profile for mapping <see cref="DoctorOffice"/> to <see cref="DoctorOfficeSummaryDto"/>.
/// </summary>
public class DoctorOfficeMapper : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DoctorOfficeMapper"/> class.
    /// Configures mapping from <see cref="DoctorOffice"/> to <see cref="DoctorOfficeSummaryDto"/>.
    /// </summary>
    public DoctorOfficeMapper()
    {
        CreateMap<DoctorOffice, DoctorOfficeSummaryDto>();
    }
}
