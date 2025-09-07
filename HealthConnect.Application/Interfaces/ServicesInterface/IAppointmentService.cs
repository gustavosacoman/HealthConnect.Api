using HealthConnect.Application.Dtos.Appointment;

namespace HealthConnect.Application.Interfaces.ServicesInterface;

public interface IAppointmentService
{
    public Task<AppointmentDetailDto> CreateAppointmentAsync(Guid clientId, AppointmentRegistrationDto appointment);

    public Task<IEnumerable<AppointmentDetailDto>> GetAppointmentsByClientIdAsync(Guid clientId);

    public Task<IEnumerable<AppointmentDetailDto>> GetAppointmentsByDoctorIdAsync(Guid doctorId);

    public Task UpdateAppointmentId(Guid Id, AppointmentUpdatingDto appointment);
    public Task<AppointmentDetailDto> GetAppointmentByIdAsync(Guid id);
}
