namespace HealthConnect.Application.Services;

using AutoMapper;
using HealthConnect.Application.Dtos.Speciality;
using HealthConnect.Application.Interfaces;
using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Models.Specialities;

/// <summary>
/// Provides handling of speciality business rules for retrieval, creation, update, and deletion.
/// </summary>
public class SpecialityService(
    ISpecialityRepository specialityRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork)
    : ISpecialityService
{
    private readonly ISpecialityRepository _specialityRepository = specialityRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    /// <inheritdoc/>
    public async Task<SpecialitySummaryDto> CreateSpeciality(SpecialityRegistrationDto specialityDto)
    {
        var existingSpeciality = await _specialityRepository.GetSpecialityByNameAsync(specialityDto.Name);
        if (existingSpeciality is not null)
        {
            throw new InvalidOperationException("Speciality with the same name already exists.");
        }

        var speciality = new Speciality
        {
            Id = Guid.NewGuid(),
            Name = specialityDto.Name,
        };

        await _specialityRepository.CreateSpecialityAsync(speciality);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<SpecialitySummaryDto>(speciality);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<SpecialitySummaryDto>> GetAllSpecialities()
    {
        var specialities = await _specialityRepository.GetAllSpecialitiesAsync();
        return _mapper.Map<IEnumerable<SpecialitySummaryDto>>(specialities);
    }

    /// <inheritdoc/>
    public async Task<SpecialitySummaryDto> GetSpecialityById(Guid id)
    {
        var speciality = await _specialityRepository.GetSpecialityByIdAsync(id);
        return _mapper.Map<SpecialitySummaryDto>(speciality);
    }

    /// <inheritdoc/>
    public async Task<SpecialitySummaryDto> GetSpecialityByName(string name)
    {
        var speciality = await _specialityRepository.GetSpecialityByNameAsync(name);
        return _mapper.Map<SpecialitySummaryDto>(speciality);
    }
}
