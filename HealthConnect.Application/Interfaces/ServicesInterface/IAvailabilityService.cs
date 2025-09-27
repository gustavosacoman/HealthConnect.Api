namespace HealthConnect.Application.Interfaces.ServicesInterface;

using HealthConnect.Application.Dtos.Availability;

/// <summary>
/// Provides methods for managing doctor availabilities.
/// </summary>
public interface IAvailabilityService
{
    /// <summary>
    /// Gets all availabilities for a specific doctor.
    /// </summary>
    /// <param name="doctorId">The unique identifier of the doctor.</param>
    /// <returns>A collection of <see cref="AvailabilitySummaryDto"/> objects.</returns>
    Task<IEnumerable<AvailabilitySummaryDto>> GetAllAvailabilitiesPerDoctorAsync(Guid doctorId);

    /// <summary>
    /// Creates a new availability slot for a doctor.
    /// </summary>
    /// <param name="availability">The details of the availability to create.</param>
    /// <returns>The created <see cref="AvailabilitySummaryDto"/>.</returns>
    Task<AvailabilitySummaryDto> CreateAvailabilityAsync(AvailabilityRegistrationDto availability);

    /// <summary>
    /// Gets the details of a specific availability slot by its identifier.
    /// </summary>
    /// <param name="availabilityId">The unique identifier of the availability slot.</param>
    /// <returns>The <see cref="AvailabilitySummaryDto"/> for the specified slot.</returns>
    Task<AvailabilitySummaryDto> GetAvailabilityByIdAsync(Guid availabilityId);

    /// <summary>
    /// Deletes a specific availability slot.
    /// </summary>
    /// <param name="availabilityId">The unique identifier of the availability slot to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAvailabilityAsync(Guid availabilityId);
}
