namespace HealthConnect.Application.Mappers;

using AutoMapper;
using HealthConnect.Application.Dtos.Speciality;
using HealthConnect.Domain.Models.Specialities;

/// <summary>
/// AutoMapper profile for mapping <see cref="Speciality"/> domain models to <see cref="SpecialitySummaryDto"/> DTOs.
/// </summary>
public class SpecialityMapper : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpecialityMapper"/> class and configures mapping rules.
    /// </summary>
    public SpecialityMapper()
    {
        CreateMap<Speciality, SpecialitySummaryDto>();
    }
}
