namespace HealthConnect.Application.Interfaces.RepositoriesInterfaces;

using HealthConnect.Domain.Models;

/// <summary>
/// Provides methods for managing and querying <see cref="Appointment"/> entities.
/// </summary>
public interface IAppointmentRepository
{
    /// <summary>
    /// Creates a new appointment asynchronously.
    /// </summary>
    /// <param name="appointment">The appointment to create.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateAppointmentAsync(Appointment appointment);

    /// <summary>
    /// Gets all appointments for a specific client asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the client.</param>
    /// <returns>A task that returns a collection of appointments.</returns>
    Task<IEnumerable<Appointment>> GetAppointmentsByClientIdAsync(Guid id);

    /// <summary>
    /// Gets all appointments for a specific doctor asynchronously.
    /// </summary>
    /// <param name="doctorId">The unique identifier of the doctor.</param>
    /// <returns>A task that returns a collection of appointments.</returns>
    Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(Guid doctorId);

    /// <summary>
    /// Gets a single appointment for a specific client asynchronously.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client.</param>
    /// <returns>A task that returns the appointment, or null if not found.</returns>
    Task<Appointment?> GetAppointmentByClientId(Guid clientId);

    /// <summary>
    /// Gets an appointment by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the appointment.</param>
    /// <returns>A task that returns the appointment, or null if not found.</returns>
    Task<Appointment?> GetAppointmentByIdAsync(Guid id);

    /// <summary>
    /// Gets a projected appointment by its unique identifier asynchronously.
    /// </summary>
    /// <typeparam name="TProjection">The type of the projection.</typeparam>
    /// <param name="id">The unique identifier of the appointment.</param>
    /// <returns>A task that returns the projected appointment.</returns>
    Task<TProjection?> GetAppointmentByIdQueryAsync<TProjection>(Guid id);
}
