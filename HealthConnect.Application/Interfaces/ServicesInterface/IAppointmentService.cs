namespace HealthConnect.Application.Interfaces.ServicesInterface;

using HealthConnect.Application.Dtos.Appointment;

/// <summary>
/// Provides appointment management services.
/// </summary>
public interface IAppointmentService
{
    /// <summary>
    /// Creates a new appointment for the specified client.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client.</param>
    /// <param name="appointment">The appointment registration details.</param>
    /// <returns>The details of the created appointment.</returns>
    Task<AppointmentDetailDto> CreateAppointmentAsync(Guid clientId, AppointmentRegistrationDto appointment);

    /// <summary>
    /// Retrieves all appointments for the specified client.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client.</param>
    /// <returns>A collection of appointment details.</returns>
    Task<IEnumerable<AppointmentDetailDto>> GetAppointmentsByClientIdAsync(Guid clientId);

    /// <summary>
    /// Retrieves all appointments for the specified doctor.
    /// </summary>
    /// <param name="doctorId">The unique identifier of the doctor.</param>
    /// <returns>A collection of appointment details.</returns>
    Task<IEnumerable<AppointmentDetailDto>> GetAppointmentsByDoctorIdAsync(Guid doctorId);

    /// <summary>
    /// Updates the appointment with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the appointment.</param>
    /// <param name="appointment">The appointment update details.</param>
    /// <returns>The <see cref="Task"/> representing the asynchronous operation.</returns>
    Task UpdateAppointmentId(Guid id, AppointmentUpdatingDto appointment);

    /// <summary>
    /// Retrieves the details of an appointment by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the appointment.</param>
    /// <returns>The details of the appointment.</returns>
    Task<AppointmentDetailDto> GetAppointmentByIdAsync(Guid id);
}
