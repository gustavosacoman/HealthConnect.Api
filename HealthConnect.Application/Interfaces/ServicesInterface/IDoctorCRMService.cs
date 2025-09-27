namespace HealthConnect.Application.Interfaces.ServicesInterface;

using HealthConnect.Application.Dtos.DoctorCRM;

/// <summary>
/// Provides methods for managing Doctor CRM records.
/// </summary>
public interface IDoctorCRMService
{
    /// <summary>
    /// Gets a Doctor CRM summary by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Doctor CRM.</param>
    /// <returns>A summary DTO of the Doctor CRM.</returns>
    Task<DoctorCRMSummaryDto> GetCRMByIdAsync(Guid id);

    /// <summary>
    /// Gets a Doctor CRM summary by CRM number and state.
    /// </summary>
    /// <param name="crmNumber">The CRM number.</param>
    /// <param name="state">The state associated with the CRM.</param>
    /// <returns>A summary DTO of the Doctor CRM.</returns>
    Task<DoctorCRMSummaryDto> GetCRMByCodeAndState(string crmNumber, string state);

    /// <summary>
    /// Gets all Doctor CRM summaries.
    /// </summary>
    /// <returns>An enumerable of Doctor CRM summary DTOs.</returns>
    Task<IEnumerable<DoctorCRMSummaryDto>> GetAllCRMAsync();

    /// <summary>
    /// Creates a new Doctor CRM record.
    /// </summary>
    /// <param name="doctorCRMDto">The registration DTO for the Doctor CRM.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateCRMAsync(DoctorCRMRegistrationDto doctorCRMDto);
}
