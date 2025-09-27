namespace HealthConnect.Application.Dtos.Doctors;

/// <summary>
/// Represents a summary of a doctor, including basic information and details about specialities and CRMs.
/// </summary>
public record DoctorSummaryDto
{
    /// <summary>
    /// Gets the unique identifier of the doctor.
    /// </summary>
    required public Guid Id { get; init; }

    /// <summary>
    /// Gets the name of the doctor.
    /// </summary>
    required public string Name { get; init; }

    /// <summary>
    /// Gets the biography of the doctor, if available.
    /// </summary>
    public string? Biography { get; init; }

    /// <summary>
    /// Gets the sex of the doctor.
    /// </summary>
    required public string Sex { get; init; }

    /// <summary>
    /// Gets the collection of the doctor's specialities.
    /// </summary>
    required public IReadOnlyCollection<DoctorSpecialityDetailDto> Specialities { get; init; }

    /// <summary>
    /// Gets the collection of the doctor's CRMs.
    /// </summary>
    required public IReadOnlyCollection<DoctorCrmDetailDto> CRMs { get; init; }

    // public string ProfilePicture { get; init; }
}
