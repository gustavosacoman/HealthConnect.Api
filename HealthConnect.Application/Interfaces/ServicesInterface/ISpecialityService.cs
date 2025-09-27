namespace HealthConnect.Application.Interfaces.ServicesInterface;

using HealthConnect.Application.Dtos.Speciality;

/// <summary>
/// Provides operations for managing medical specialities.
/// </summary>
public interface ISpecialityService
{
    /// <summary>
    /// Gets a speciality by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the speciality.</param>
    /// <returns>A <see cref="SpecialitySummaryDto"/> representing the speciality.</returns>
    Task<SpecialitySummaryDto> GetSpecialityById(Guid id);

    /// <summary>
    /// Gets all available specialities.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="SpecialitySummaryDto"/>.</returns>
    Task<IEnumerable<SpecialitySummaryDto>> GetAllSpecialities();

    /// <summary>
    /// Gets a speciality by its name.
    /// </summary>
    /// <param name="name">The name of the speciality.</param>
    /// <returns>A <see cref="SpecialitySummaryDto"/> representing the speciality.</returns>
    Task<SpecialitySummaryDto> GetSpecialityByName(string name);

    /// <summary>
    /// Creates a new speciality.
    /// </summary>
    /// <param name="specialityDto">The registration details for the new speciality.</param>
    /// <returns>A <see cref="SpecialitySummaryDto"/> representing the created speciality.</returns>
    Task<SpecialitySummaryDto> CreateSpeciality(SpecialityRegistrationDto specialityDto);
}
