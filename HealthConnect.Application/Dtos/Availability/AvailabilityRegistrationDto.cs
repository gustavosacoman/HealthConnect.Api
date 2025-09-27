namespace HealthConnect.Application.Dtos.Availability;

/// <summary>
/// Data transfer object for registering a doctor's availability slot.
/// </summary>
public record AvailabilityRegistrationDto
{
    /// <summary>
    /// Gets the unique identifier of the doctor.
    /// </summary>
    required public Guid DoctorId { get; init; }

    /// <summary>
    /// Gets the date and time of the availability slot.
    /// </summary>
    required public DateTime SlotDateTime { get; init; }

    /// <summary>
    /// Gets the duration of the slot in minutes.
    /// </summary>
    required public int DurationMinutes { get; init; }
}
