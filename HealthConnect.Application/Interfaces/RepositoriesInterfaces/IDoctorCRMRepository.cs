namespace HealthConnect.Application.Interfaces.RepositoriesInterfaces;

using HealthConnect.Domain.Models;

/// <summary>
/// Repository interface for managing DoctorCRM entities.
/// </summary>
public interface IDoctorCRMRepository
{
    /// <summary>
    /// Retrieves a DoctorCRM entity by its CRM code and state.
    /// </summary>
    /// <param name="code">The CRM code to search for.</param>
    /// <param name="state">The state associated with the CRM.</param>
    /// <returns>The matching <see cref="DoctorCRM"/> entity, or null if not found.</returns>
    Task<DoctorCRM?> GetCRMByCodeAndState(string code, string state);

    /// <summary>
    /// Gets a queryable collection of all DoctorCRM entities.
    /// </summary>
    /// <returns>An <see cref="IQueryable{DoctorCRM}"/> representing all DoctorCRM entities.</returns>
    IQueryable<DoctorCRM> GetAllCRMAsync();

    /// <summary>
    /// Retrieves a DoctorCRM entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the DoctorCRM.</param>
    /// <returns>The matching <see cref="DoctorCRM"/> entity, or null if not found.</returns>
    Task<DoctorCRM?> GetByIdAsync(Guid id);

    /// <summary>
    /// Creates a new DoctorCRM entity.
    /// </summary>
    /// <param name="doctorCRM">The <see cref="DoctorCRM"/> entity to create.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateCRMAsync(DoctorCRM doctorCRM);
}
