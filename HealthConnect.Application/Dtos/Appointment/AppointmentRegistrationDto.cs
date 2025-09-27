namespace HealthConnect.Application.Dtos.Appointment;

/// <summary>
/// Data transfer object for registering an appointment.
/// </summary>
public record AppointmentRegistrationDto
{
    /// <summary>
    /// Gets the identifier of the availability slot for the appointment.
    /// </summary>
    public Guid AvailabilityId { get; init; }

    /// <summary>
    /// Gets any additional notes for the appointment.
    /// </summary>
    public string? Notes { get; init; }
}
