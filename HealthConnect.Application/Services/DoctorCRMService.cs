namespace HealthConnect.Application.Services;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using HealthConnect.Application.Dtos.DoctorCRM;
using HealthConnect.Application.Interfaces;
using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Provides handling of doctorCrmS business rules for retrieval, creation, update, and deletion.
/// </summary>
public class DoctorCRMService(
    IDoctorCRMRepository doctorCRMRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IDoctorRepository doctorRepository)
    : IDoctorCRMService
{
    private readonly IDoctorCRMRepository _doctorCRMRepository = doctorCRMRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDoctorRepository _doctorRepository = doctorRepository;

    /// <inheritdoc/>
    public async Task CreateCRMAsync(DoctorCRMRegistrationDto doctorCRMDto)
    {
        var exist = await _doctorCRMRepository.GetCRMByCodeAndState(doctorCRMDto.CRMNumber, doctorCRMDto.State);

        if (exist != null)
        {
            throw new ArgumentException($"The CRM {doctorCRMDto.CRMNumber}" +
                $" with a state {doctorCRMDto.State} already exist in database");
        }

        var doctor = await _doctorRepository.GetDoctorById(doctorCRMDto.DoctorId) 
            ?? throw new KeyNotFoundException($"No doctor found with id {doctorCRMDto.DoctorId}");

        var newDoctorCRM = new DoctorCRM
        {
            Id = Guid.NewGuid(),
            DoctorId = doctorCRMDto.DoctorId,
            CRMNumber = doctorCRMDto.CRMNumber,
            State = doctorCRMDto.State,
            Doctor = doctor,
        };

        await _doctorCRMRepository.CreateCRMAsync(newDoctorCRM);
        await _unitOfWork.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<DoctorCRMSummaryDto> GetCRMByCodeAndState(string crmNumber, string state)
    {
        if (string.IsNullOrWhiteSpace(crmNumber))
        {
            throw new ArgumentException("CRM number must be provided", nameof(crmNumber));
        }

        if (string.IsNullOrWhiteSpace(state))
        {
            throw new ArgumentException("State must be provided", nameof(state));
        }

        var crm = await _doctorCRMRepository.GetCRMByCodeAndState(crmNumber, state) ??
            throw new KeyNotFoundException($"No CRM found with number {crmNumber} in state {state}");

        return _mapper.Map<DoctorCRMSummaryDto>(crm);
    }

    /// <inheritdoc/>
    public async Task<DoctorCRMSummaryDto> GetCRMByIdAsync(Guid id)
    {
        var crm = await _doctorCRMRepository.GetByIdAsync(id) ??
            throw new KeyNotFoundException($"No CRM found with id {id}");
        return _mapper.Map<DoctorCRMSummaryDto>(crm);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<DoctorCRMSummaryDto>> GetAllCRMAsync()
    {
        var queriable = _doctorCRMRepository.GetAllCRMAsync();

        var crms = queriable
            .OrderBy(c => c.State)
            .ThenBy(c => c.CRMNumber)
            .ProjectTo<DoctorCRMSummaryDto>(_mapper.ConfigurationProvider);

        return await crms.ToListAsync();
    }

}
