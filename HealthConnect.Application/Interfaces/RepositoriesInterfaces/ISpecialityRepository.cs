namespace HealthConnect.Application.Interfaces.RepositoriesInterfaces;

using HealthConnect.Domain.Models.Specialities;

/// <summary>
/// Provides methods for accessing and managing <see cref="Speciality"/> entities.
/// </summary>
public interface ISpecialityRepository
{
    /// <summary>
    /// Gets a <see cref="Speciality"/> by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the speciality.</param>
    /// <returns>The <see cref="Speciality"/> if found; otherwise, null.</returns>
    public Task<Speciality?> GetSpecialityByIdAsync(Guid id);

    /// <summary>
    /// Gets all <see cref="Speciality"/> entities.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="Speciality"/>.</returns>
    public Task<IEnumerable<Speciality>> GetAllSpecialitiesAsync();

    /// <summary>
    /// Creates a new <see cref="Speciality"/>.
    /// </summary>
    /// <param name="speciality">The speciality to create.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task CreateSpecialityAsync(Speciality speciality);

    /// <summary>
    /// Gets a <see cref="Speciality"/> by its name.
    /// </summary>
    /// <param name="name">The name of the speciality.</param>
    /// <returns>The <see cref="Speciality"/> if found; otherwise, null.</returns>
    public Task<Speciality?> GetSpecialityByNameAsync(string name);

    /// <summary>
    /// Gets all <see cref="Speciality"/> entities associated with a specific doctor.
    /// </summary>
    /// <param name="doctorId">The unique identifier of the doctor.</param>
    /// <returns>An enumerable collection of <see cref="Speciality"/> for the specified doctor.</returns>
    Task<IEnumerable<Speciality>> GetSpecialitiesForDoctor(Guid doctorId);

    /// <summary>
    /// Gets a <see cref="DoctorSpeciality"/> by its RQE number.
    /// </summary>
    /// <param name="rqeNumber">The RQE number of the doctor speciality.</param>
    /// <returns>The <see cref="DoctorSpeciality"/> if found; otherwise, null.</returns>
    public Task<DoctorSpeciality?> GetDoctorSpecialityByRqe(string rqeNumber);
}
