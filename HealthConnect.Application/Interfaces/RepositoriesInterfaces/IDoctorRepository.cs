namespace HealthConnect.Application.Interfaces.RepositoriesInterfaces;

using HealthConnect.Domain.Models;
using HealthConnect.Domain.Models.Specialities;

/// <summary>
/// Repository interface for managing <see cref="Doctor"/> entities and their relationships with specialities.
/// </summary>
public interface IDoctorRepository
{
    /// <summary>
    /// Gets all doctors.
    /// </summary>
    /// <returns>An <see cref="IQueryable{Doctor}"/> of all doctors.</returns>
    IQueryable<Doctor> GetAllDoctors();

    /// <summary>
    /// Gets a doctor by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the doctor.</param>
    /// <returns>A <see cref="Task{Doctor}"/> representing the asynchronous operation. The result contains the doctor if found; otherwise, null.</returns>
    Task<Doctor?> GetDoctorById(Guid id);

    /// <summary>
    /// Gets a doctor by their RQE (Registro de Qualificação de Especialista).
    /// </summary>
    /// <param name="rqe">The RQE of the doctor.</param>
    /// <returns>A <see cref="Task{Doctor}"/> representing the asynchronous operation. The result contains the doctor if found; otherwise, null.</returns>
    Task<Doctor?> GetDoctorByRQE(string rqe);

    /// <summary>
    /// Creates a new doctor.
    /// </summary>
    /// <param name="doctor">The doctor entity to create.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task CreateDoctor(Doctor doctor);

    /// <summary>
    /// Gets all doctors by speciality.
    /// </summary>
    /// <param name="specialityId">The unique identifier of the speciality.</param>
    /// <returns>An <see cref="IQueryable{Doctor}"/> of doctors with the specified speciality.</returns>
    IQueryable<Doctor> GetAllDoctorsBySpecialityAsync(Guid specialityId);

    /// <summary>
    /// Adds a link between a doctor and a speciality.
    /// </summary>
    /// <param name="doctorSpeciality">The doctor-speciality link to add.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddDoctorLinkToSpeciality(DoctorSpeciality doctorSpeciality);

    /// <summary>
    /// Removes a link between a doctor and a speciality.
    /// </summary>
    /// <param name="doctorSpeciality">The doctor-speciality link to remove.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    void RemoveDoctorLinkToSpeciality(DoctorSpeciality doctorSpeciality);

    /// <summary>
    /// Gets the link between a doctor and a speciality.
    /// </summary>
    /// <param name="doctorId">The unique identifier of the doctor.</param>
    /// <param name="specialityId">The unique identifier of the speciality.</param>
    /// <returns>A <see cref="Task{DoctorSpeciality}"/> representing the asynchronous operation. The result contains the link if found.</returns>
    Task<DoctorSpeciality?> GetDoctorSpecialityLink(Guid doctorId, Guid specialityId);

    /// <summary>
    /// Gets a doctor by their user identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A <see cref="Task{Doctor}"/> representing the asynchronous operation. The result contains the doctor if found; otherwise, null.</returns>
    Task<Doctor?> GetDoctorByUserId(Guid userId);
}
