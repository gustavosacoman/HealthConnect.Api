namespace HealthConnect.Application.Dtos.Speciality;

/// <summary>
/// Data Transfer Object for registering a medical speciality.
/// </summary>
public record SpecialityRegistrationDto
{
    /// <summary>
    /// Gets the name of the speciality.
    /// </summary>
    required public string Name { get; init; }

}