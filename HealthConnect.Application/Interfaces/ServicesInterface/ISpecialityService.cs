using HealthConnect.Application.Dtos.Speciality;

namespace HealthConnect.Application.Interfaces.ServicesInterface;

public interface ISpecialityService
{
    public Task<SpecialitySummaryDto> GetSpecialityById(Guid id);

    public Task<IEnumerable<SpecialitySummaryDto>> GetAllSpecialities();

    public Task<SpecialitySummaryDto> GetSpecialityByName(string name);

    public Task<SpecialitySummaryDto> CreateSpeciality(SpecialityRegistrationDto specialityDto);

}
