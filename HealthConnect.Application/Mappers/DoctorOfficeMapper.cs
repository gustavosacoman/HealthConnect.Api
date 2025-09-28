namespace HealthConnect.Application.Mappers;

using AutoMapper;
using HealthConnect.Application.Dtos.DoctorOffice;
using HealthConnect.Domain.Models;

/// <summary>
/// AutoMapper profile for mapping <see cref="DoctorOffice"/> domain models to <see cref="DoctorOfficeSummaryDto"/> DTOs.
/// </summary>
public class DoctorOfficeMapper : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DoctorOfficeMapper"/> class.
    /// Configures mapping between <see cref="DoctorOffice"/> and <see cref="DoctorOfficeSummaryDto"/>.
    /// </summary>
    public DoctorOfficeMapper()
    {
        CreateMap<DoctorOffice, DoctorOfficeSummaryDto>();
    }
}
