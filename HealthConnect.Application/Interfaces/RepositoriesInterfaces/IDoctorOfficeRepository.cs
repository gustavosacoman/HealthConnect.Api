namespace HealthConnect.Application.Interfaces.RepositoriesInterfaces;

using HealthConnect.Domain.Models;

/// <summary>
/// Provides methods for accessing and managing <see cref="DoctorOffice"/> entities in the data store.
/// </summary>
public interface IDoctorOfficeRepository
{
    /// <summary>
    /// Retrieves a <see cref="DoctorOffice"/> by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the doctor office.</param>
    /// <returns>The <see cref="DoctorOffice"/> if found; otherwise, <c>null</c>.</returns>
    Task<DoctorOffice?> GetDoctorOfficeByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all <see cref="DoctorOffice"/> entities.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="DoctorOffice"/> entities.</returns>
    Task<IEnumerable<DoctorOffice>> GetAllDoctorOfficesAsync();

    /// <summary>
    /// Retrieves all <see cref="DoctorOffice"/> entities associated with a specific doctor by their unique identifier.
    /// </summary>
    /// <param name="doctorId">The unique identifier of the doctor.</param>
    /// <returns>
    /// An enumerable collection of <see cref="DoctorOffice"/> entities associated with the specified doctor.
    /// </returns>
    Task<IEnumerable<DoctorOffice>> GetOfficeByDoctorIdAsync(Guid doctorId);

    /// <summary>
    /// Retrieves the primary <see cref="DoctorOffice"/> for a specific doctor by their unique identifier.
    /// </summary>
    /// <param name="doctorId">The unique identifier of the doctor.</param>
    /// <returns>
    /// The primary <see cref="DoctorOffice"/> associated with the specified doctor if found; otherwise, <c>null</c>.
    /// </returns>
    Task<DoctorOffice?> GetPrimaryOfficeByDoctorIdAsync(Guid doctorId);

    /// <summary>
    /// Creates a new <see cref="DoctorOffice"/> entity in the data store.
    /// </summary>
    /// <param name="doctorOffice">The <see cref="DoctorOffice"/> entity to create.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateDoctorOfficeAsync(DoctorOffice doctorOffice);
}
