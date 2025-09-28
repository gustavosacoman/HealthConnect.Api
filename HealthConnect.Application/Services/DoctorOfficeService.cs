namespace HealthConnect.Application.Services;

using AutoMapper;
using HealthConnect.Application.Dtos.DoctorOffice;
using HealthConnect.Application.Interfaces;
using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Models;

/// <summary>
/// Provides handling of Doctor offices business rules for retrieval, creation, update, and deletion.
/// </summary>
public class DoctorOfficeService(
    IDoctorOfficeRepository doctorOfficeRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IDoctorRepository doctorRepository)
    : IDoctorOfficeService
{
    private readonly IDoctorOfficeRepository _doctorOfficeRepository = doctorOfficeRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDoctorRepository _doctorRepository = doctorRepository;

    /// <inheritdoc/>
    public async Task<DoctorOfficeSummaryDto?> GetOfficeByIdAsync(Guid id)
    {
        var doctorOffice = await _doctorOfficeRepository.GetDoctorOfficeByIdAsync(id) ??
            throw new NullReferenceException("Doctor office not found.");

        return _mapper.Map<DoctorOfficeSummaryDto>(doctorOffice);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<DoctorOfficeSummaryDto>> GetAllDoctorOfficeAsync()
    {
        var doctorOffices = await _doctorOfficeRepository.GetAllDoctorOfficesAsync();
        return _mapper.Map<IEnumerable<DoctorOfficeSummaryDto>>(doctorOffices);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<DoctorOfficeSummaryDto>> GetOfficeByDoctorIdAsync(Guid doctorId)
    {
        var doctorOffices = await _doctorOfficeRepository.GetOfficeByDoctorIdAsync(doctorId) ??
            throw new NullReferenceException("Doctor office not found.");

        return _mapper.Map<IEnumerable<DoctorOfficeSummaryDto>>(doctorOffices);
    }

    /// <inheritdoc/>
    public async Task<DoctorOfficeSummaryDto?> GetPrimaryOfficeByDoctorIdAsync(Guid doctorId)
    {
        var doctorOffice = await _doctorOfficeRepository.GetPrimaryOfficeByDoctorIdAsync(doctorId) ??
            throw new NullReferenceException("Doctor primary office not found.");

        return _mapper.Map<DoctorOfficeSummaryDto>(doctorOffice);
    }

    /// <inheritdoc/>
    public async Task<DoctorOfficeSummaryDto> CreateDoctorOfficeAsync(DoctorOfficeRegistrationDto doctorOfficeRegistration)
    {
        if (doctorOfficeRegistration.IsPrimary)
        {
            var primaryOfficeExists = await _doctorOfficeRepository.GetPrimaryOfficeByDoctorIdAsync(doctorOfficeRegistration.DoctorId);
            if (primaryOfficeExists != null)
            {
                throw new InvalidOperationException("A primary office already exists for this doctor. Please set the existing primary office to non-primary before assigning a new one.");
            }
        }

        var doctor = await _doctorRepository.GetDoctorById(doctorOfficeRegistration.DoctorId) ??
            throw new NullReferenceException("Doctor not found.");

        var doctorOffice = new DoctorOffice
        {
            Id = Guid.NewGuid(),
            DoctorId = doctorOfficeRegistration.DoctorId,
            Street = doctorOfficeRegistration.Street,
            Number = doctorOfficeRegistration.Number,
            Complement = doctorOfficeRegistration.Complement,
            State = doctorOfficeRegistration.State,
            ZipCode = doctorOfficeRegistration.ZipCode,
            Phone = doctorOfficeRegistration.Phone,
            SecretaryPhone = doctorOfficeRegistration.SecretaryPhone,
            SecretaryEmail = doctorOfficeRegistration.SecretaryEmail,
            IsPrimary = doctorOfficeRegistration.IsPrimary,
            Doctor = doctor,
        };

        await _doctorOfficeRepository.CreateDoctorOfficeAsync(doctorOffice);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<DoctorOfficeSummaryDto>(doctorOffice);
    }
}
