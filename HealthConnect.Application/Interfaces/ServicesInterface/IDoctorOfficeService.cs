namespace HealthConnect.Application.Interfaces.ServicesInterface;

using HealthConnect.Application.Dtos.DoctorOffice;

/// <summary>
/// Provides operations for managing doctor offices.
/// </summary>
public interface IDoctorOfficeService
{
    /// <summary>
    /// Gets the summary information for a doctor office by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the doctor office.</param>
    /// <returns>
    /// A <see cref="DoctorOfficeSummaryDto"/> representing the office, or <c>null</c> if not found.
    /// </returns>
    Task<DoctorOfficeSummaryDto?> GetOfficeByIdAsync(Guid id);

    /// <summary>
    /// Gets summary information for all doctor offices.
    /// </summary>
    /// <returns>
    /// An enumerable collection of <see cref="DoctorOfficeSummaryDto"/>.
    /// </returns>
    Task<IEnumerable<DoctorOfficeSummaryDto>> GetAllDoctorOfficeAsync();

    /// <summary>
    /// Gets summary information for all offices associated with a specific doctor.
    /// </summary>
    /// <param name="doctorId">The unique identifier of the doctor.</param>
    /// <returns>
    /// An enumerable collection of <see cref="DoctorOfficeSummaryDto"/>.
    /// </returns>
    Task<IEnumerable<DoctorOfficeSummaryDto>> GetOfficeByDoctorIdAsync(Guid doctorId);

    /// <summary>
    /// Gets the primary office for a specific doctor.
    /// </summary>
    /// <param name="doctorId">The unique identifier of the doctor.</param>
    /// <returns>
    /// A <see cref="DoctorOfficeSummaryDto"/> representing the primary office, or <c>null</c> if not found.
    /// </returns>
    Task<DoctorOfficeSummaryDto?> GetPrimaryOfficeByDoctorIdAsync(Guid doctorId);

    /// <summary>
    /// Creates a new doctor office.
    /// </summary>
    /// <param name="doctorOfficeRegistration">The registration details for the new doctor office.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<DoctorOfficeSummaryDto> CreateDoctorOfficeAsync(DoctorOfficeRegistrationDto doctorOfficeRegistration);
}
