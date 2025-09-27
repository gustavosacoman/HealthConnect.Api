namespace HealthConnect.Application.Interfaces.ServicesInterface;

using HealthConnect.Application.Dtos.Doctors;

/// <summary>
/// Provides methods for managing and retrieving doctor information.
/// </summary>
public interface IDoctorService
{
    /// <summary>
    /// Retrieves all doctors with summary information.
    /// </summary>
    /// <returns>A collection of <see cref="DoctorSummaryDto"/>.</returns>
    Task<IEnumerable<DoctorSummaryDto>> GetAllDoctorsAsync();

    /// <summary>
    /// Retrieves detailed information for a doctor by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the doctor.</param>
    /// <returns>A <see cref="DoctorDetailDto"/> containing detailed doctor information.</returns>
    Task<DoctorDetailDto> GetDoctorByIdDetailAsync(Guid id);

    /// <summary>
    /// Retrieves summary information for a doctor by their RQE (Registro de Qualificação de Especialista).
    /// </summary>
    /// <param name="rqe">The RQE of the doctor.</param>
    /// <returns>A <see cref="DoctorSummaryDto"/> containing summary doctor information.</returns>
    Task<DoctorSummaryDto> GetDoctorByRQEAsync(string rqe);

    /// <summary>
    /// Retrieves summary information for a doctor by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the doctor.</param>
    /// <returns>A <see cref="DoctorSummaryDto"/> containing summary doctor information.</returns>
    Task<DoctorSummaryDto> GetDoctorByIdAsync(Guid id);

    /// <summary>
    /// Updates the information of a doctor.
    /// </summary>
    /// <param name="id">The unique identifier of the doctor.</param>
    /// <param name="doctorUpdatingDto">The updated doctor information.</param>
    /// <returns>A <see cref="DoctorSummaryDto"/> containing the updated summary information.</returns>
    Task<DoctorSummaryDto> UpdateDoctorAsync(Guid id, DoctorUpdatingDto doctorUpdatingDto);

    /// <summary>
    /// Retrieves all doctors with detailed information by their speciality.
    /// </summary>
    /// <param name="specialityId">The unique identifier of the speciality.</param>
    /// <returns>A collection of <see cref="DoctorDetailDto"/>.</returns>
    Task<IEnumerable<DoctorDetailDto>> GetAllDoctorsBySpecialityAsync(Guid specialityId);

    /// <summary>
    /// Retrieves detailed information for a doctor by their user identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A <see cref="DoctorDetailDto"/> containing detailed doctor information.</returns>
    Task<DoctorDetailDto> GetDoctoDetailByUserIdAsync(Guid userId);
}
