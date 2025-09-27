using AutoMapper;
using HealthConnect.Application.Dtos.Appointment;
using HealthConnect.Application.Interfaces;
using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Enum;
using HealthConnect.Domain.Models;

namespace HealthConnect.Application.Services;

public class AppointmentService(
    IAppointmentRepository appointmentRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IAvailabilityRepository availabilityRepository,
    IClientRepository clientRepository,
    IDoctorRepository doctorRepository)
    : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IAvailabilityRepository _availabilityRepository = availabilityRepository;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IDoctorRepository _doctorRepository = doctorRepository;

    public async Task<AppointmentDetailDto> CreateAppointmentAsync(Guid clientId, AppointmentRegistrationDto appointment)
    {
        var availability = await _availabilityRepository.GetAvailabilityByIdAsync(appointment.AvailabilityId) ??
                           throw new KeyNotFoundException("Availability not found.");

        if (availability.IsBooked)
        {
            throw new InvalidOperationException("The selected time slot is already booked.");
        }

        if (availability.SlotDateTime < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Cannot book an appointment in the past.");
        }

        var client = await _clientRepository.GetClientByIdAsync(clientId) ??
                     throw new KeyNotFoundException("Client not found.");

        var doctor = await _doctorRepository.GetDoctorById(availability.DoctorId) ??
                     throw new KeyNotFoundException("Doctor not found.");

        var newAppointment = new Appointment
        {
            Id = Guid.NewGuid(),
            ClientId = client.Id,
            DoctorId = availability.DoctorId,
            Client = client,
            AvailabilityId = availability.Id,
            AppointmentDateTime = availability.SlotDateTime,
            AppointmentStatus = AppointmentStatus.Scheduled,
            Notes = appointment.Notes,
            Doctor = doctor,
            Availability = availability,
        };

        await _appointmentRepository.CreateAppointmentAsync(newAppointment);

        availability.IsBooked = true;
        await _unitOfWork.SaveChangesAsync();

        var getAppointment = await _appointmentRepository.GetAppointmentByIdQueryAsync<AppointmentDetailDto>(newAppointment.Id);

        return getAppointment;

    }

    public async Task<IEnumerable<AppointmentDetailDto>> GetAppointmentsByClientIdAsync(Guid clientId)
    {
        var appointments = await _appointmentRepository.GetAppointmentsByClientIdAsync(clientId);
        return _mapper.Map<IEnumerable<AppointmentDetailDto>>(appointments);
    }

    public async Task<IEnumerable<AppointmentDetailDto>> GetAppointmentsByDoctorIdAsync(Guid doctorId)
    {
        var appointments = await _appointmentRepository.GetAppointmentsByDoctorIdAsync(doctorId);
        return _mapper.Map<IEnumerable<AppointmentDetailDto>>(appointments);
    }

    public async Task<AppointmentDetailDto> GetAppointmentByIdAsync(Guid id)
    {
        var existingAppointment = await _appointmentRepository.GetAppointmentByIdAsync(id);
        if (existingAppointment == null)
        {
            throw new KeyNotFoundException("Appointment not found.");
        }
        return _mapper.Map<AppointmentDetailDto>(existingAppointment);
    }

    public async Task UpdateAppointmentId(Guid Id, AppointmentUpdatingDto appointment)
    {
        var existingAppointment = await _appointmentRepository.GetAppointmentByIdAsync(Id);
        if (existingAppointment == null)
        {
            throw new KeyNotFoundException("Appointment not found.");
        }

        existingAppointment.Notes = appointment.Notes ?? existingAppointment.Notes;
        existingAppointment.AppointmentStatus = appointment.Status ?? existingAppointment.AppointmentStatus;
        existingAppointment.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
    }

}
