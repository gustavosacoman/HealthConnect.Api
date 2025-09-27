namespace HealthConnect.Application.Dtos.Availability;

using HealthConnect.Application.Dtos.Doctors;

/// <summary>
/// Represents a summary of a doctor's availability slot.
/// </summary>
public record AvailabilitySummaryDto
{
    /// <summary>
    /// Gets the unique identifier for the availability slot.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the unique identifier of the doctor associated with this availability.
    /// </summary>
    public Guid DoctorId { get; init; }

    /// <summary>
    /// Gets the name of the doctor.
    /// </summary>
    required public string Name { get; init; }

    /// <summary>
    /// Gets the primary specialty of the doctor.
    /// </summary>
    required public string Specialty { get; init; }

    /// <summary>
    /// Gets the collection of specialty details for the doctor.
    /// </summary>
    required public IReadOnlyCollection<DoctorSpecialityDetailDto> Specialities { get; init; }

    /// <summary>
    /// Gets the date and time of the availability slot.
    /// </summary>
    public DateTime SlotDateTime { get; init; }

    /// <summary>
    /// Gets the duration of the availability slot in minutes.
    /// </summary>
    public int DurationMinutes { get; init; }
}
