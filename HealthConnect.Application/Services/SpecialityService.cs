using AutoMapper;
using HealthConnect.Application.Dtos.Speciality;
using HealthConnect.Application.Interfaces;
using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Models;
using HealthConnect.Domain.Models.Specialities;
using System.Runtime.InteropServices;

namespace HealthConnect.Application.Services;

public class SpecialityService(
    ISpecialityRepository specialityRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork) : ISpecialityService
{
    private readonly ISpecialityRepository _specialityRepository = specialityRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<SpecialitySummaryDto> CreateSpeciality(SpecialityRegistrationDto specialityDto)
    {
        var existingSpeciality = await _specialityRepository.GetSpecialityByNameAsync(specialityDto.Name);
        if (existingSpeciality != null)
        {
            throw new InvalidOperationException("Speciality with the same name already exists.");
        }

        var speciality = new Speciality
        {
            Id = Guid.NewGuid(),
            Name = specialityDto.Name
        };

        await _specialityRepository.CreateSpecialityAsync(speciality);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<SpecialitySummaryDto>(speciality);
    }

    public async Task<IEnumerable<SpecialitySummaryDto>> GetAllSpecialities()
    {
        var specialities = await _specialityRepository.GetAllSpecialitiesAsync();
        return _mapper.Map<IEnumerable<SpecialitySummaryDto>>(specialities);
    }

    public async Task<SpecialitySummaryDto> GetSpecialityById(Guid id)
    {
        var speciality = await _specialityRepository.GetSpecialityByIdAsync(id);
        return _mapper.Map<SpecialitySummaryDto>(speciality);
    }

    public async Task<SpecialitySummaryDto> GetSpecialityByName(string name)
    {
        var speciality = await _specialityRepository.GetSpecialityByNameAsync(name);
        return _mapper.Map<SpecialitySummaryDto>(speciality);
    }
}
