using HealthConnect.Domain.Models;

namespace HealthConnect.Application.Interfaces.RepositoriesInterfaces;

public interface IAppointmentRepository
{
    public Task CreateAppointmentAsync(Appointment appointment);

    public Task<IEnumerable<Appointment>> GetAppointmentsByClientIdAsync(Guid id);

    public Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(Guid doctorId);

    public Task<Appointment?> GetAppointmentByClientId(Guid clientId);

    public Task<Appointment?> GetAppointmentByIdAsync(Guid id);

    public Task<TProjection> GetAppointmentByIdQueryAsync<TProjection>(Guid id);

}
