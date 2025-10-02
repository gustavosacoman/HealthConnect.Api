namespace HealthConnect.Application.Services;

using AutoMapper;
using HealthConnect.Application.Dtos.Availability;
using HealthConnect.Application.Interfaces;
using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Models;

/// <summary>
/// Provides handling of availability business rules for retrieval, creation, update, and deletion.
/// </summary>
public class AvailabilityService
    (IAvailabilityRepository availabilityRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IDoctorRepository doctorRepository,
    IDoctorOfficeRepository doctorOfficeRepository)
    : IAvailabilityService
{
    private readonly IAvailabilityRepository _availabilityRepository = availabilityRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDoctorRepository _doctorRepository = doctorRepository;
    private readonly IDoctorOfficeRepository _doctorOfficeRepository = doctorOfficeRepository;

    /// <inheritdoc/>
    public async Task<AvailabilitySummaryDto> CreateAvailabilityAsync(AvailabilityRegistrationDto availability)
    {
        var doctor = await _doctorRepository.GetDoctorById(availability.DoctorId) ??
            throw new ArgumentNullException("Doctor not found");

        var cleanDatePrecion =
            new DateTime(
                availability.SlotDateTime.Year,
                availability.SlotDateTime.Month,
                availability.SlotDateTime.Day,
                availability.SlotDateTime.Hour,
                availability.SlotDateTime.Minute,
                0,
                availability.SlotDateTime.Kind);
        var newSlotEnd = cleanDatePrecion.AddMinutes(availability.DurationMinutes);

        var hasConflit = await _availabilityRepository.HasOverlappingAvailabilityAsync(
            availability.DoctorId,
            cleanDatePrecion,
            newSlotEnd);

        if (cleanDatePrecion < DateTime.UtcNow)
        {
            throw new InvalidOperationException("The proposed time slot is in the past.");
        }

        if (hasConflit)
        {
            throw new
                InvalidOperationException("The proposed time slot overlaps" +
                " with an existing availability.");
        }

        if (availability.DurationMinutes <= 0)
        {
            throw new InvalidOperationException("Duration must be greater than zero.");
        }

        DoctorOffice? doctorOffice = null;

        if (availability.DoctorOfficeId.HasValue && availability.DoctorOfficeId.Value != Guid.Empty)
        {
            doctorOffice = await _doctorOfficeRepository.GetDoctorOfficeByIdAsync(availability.DoctorOfficeId.Value)
                ?? throw new KeyNotFoundException("The specified Doctor office was not found.");
        }

        var newAvailability = new Availability
        {
            Id = Guid.NewGuid(),
            DoctorId = availability.DoctorId,
            Doctor = doctor,
            SlotDateTime = cleanDatePrecion,
            DurationMinutes = availability.DurationMinutes,
            IsBooked = false,
            DoctorOffice = doctorOffice,
        };

        await _availabilityRepository.CreateAvailabilityAsync(newAvailability);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<AvailabilitySummaryDto>(newAvailability);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AvailabilitySummaryDto>> GetAllAvailabilitiesPerDoctorAsync(Guid doctorId)
    {
        if (doctorId == Guid.Empty)
        {
            throw new ArgumentNullException("Invalid doctor ID");
        }

        if (await _doctorRepository.GetDoctorById(doctorId) is null)
        {
            throw new ArgumentNullException("Doctor not found");
        }

        return await _availabilityRepository
            .GetAllAvailabilityPerDoctor<AvailabilitySummaryDto>(doctorId);
    }

    /// <inheritdoc/>
    public async Task<AvailabilitySummaryDto> GetAvailabilityByIdAsync(Guid availabilityId)
    {
        if (availabilityId == Guid.Empty)
        {
            throw new ArgumentNullException("Invalid availability ID");
        }

        return await _availabilityRepository
            .GetAvailabilityByIdAsync(availabilityId) is Availability availability
            ? _mapper.Map<AvailabilitySummaryDto>(availability)
            : throw new ArgumentNullException("Availability not found");
    }

    /// <inheritdoc/>
    public async Task DeleteAvailabilityAsync(Guid availabilityId)
    {
        if (availabilityId == Guid.Empty)
        {
            throw new ArgumentNullException("Invalid availability ID");
        }

        var availability = await _availabilityRepository
            .GetAvailabilityByIdAsync(availabilityId)
            ?? throw new ArgumentNullException("Availability not found");

        await _availabilityRepository.DeleteAvailability(availability);
        await _unitOfWork.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AvailabilitySummaryDto>> CreateMultipleAvailabilitiesAsync(IEnumerable<AvailabilityRegistrationDto> availabilities)
    {
        var doctorId = availabilities.First().DoctorId;
        if (availabilities.Any(a => a.DoctorId != doctorId))
        {
            throw new InvalidOperationException("All availability slots in a bulk request must belong to the same doctor.");
        }

        var doctor = await _doctorRepository.GetDoctorById(doctorId) ??
            throw new ArgumentNullException("Doctor not found");

        var newAvailabilityEntities = new List<Availability>();

        foreach (var availabilityDto in availabilities)
        {
            var cleanDatePrecion =
            new DateTime(
                availabilityDto.SlotDateTime.Year,
                availabilityDto.SlotDateTime.Month,
                availabilityDto.SlotDateTime.Day,
                availabilityDto.SlotDateTime.Hour,
                availabilityDto.SlotDateTime.Minute,
                0,
                availabilityDto.SlotDateTime.Kind);
            var newSlotEnd = cleanDatePrecion.AddMinutes(availabilityDto.DurationMinutes);

            if (cleanDatePrecion < DateTime.UtcNow)
            {
                throw new InvalidOperationException($"The proposed time slot {cleanDatePrecion} is in the past.");
            }

            if (availabilityDto.DurationMinutes <= 0)
            {
                throw new InvalidOperationException("Duration must be greater than zero.");
            }

            var hasConflit = await _availabilityRepository.HasOverlappingAvailabilityAsync(
                availabilityDto.DoctorId,
                cleanDatePrecion,
                newSlotEnd);

            if (hasConflit)
            {
                throw new InvalidOperationException($"The proposed time slot {cleanDatePrecion} overlaps with an existing availability.");
            }

            var newAvailability = new Availability
            {
                Id = Guid.NewGuid(),
                DoctorId = doctor.Id,
                Doctor = doctor,
                SlotDateTime = cleanDatePrecion,
                DurationMinutes = availabilityDto.DurationMinutes,
                IsBooked = false,
            };

            newAvailabilityEntities.Add(newAvailability);
        }

        await _availabilityRepository.CreateMultipleAvailabilitiesAsync(newAvailabilityEntities);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<IEnumerable<AvailabilitySummaryDto>>(newAvailabilityEntities);
    }
}
