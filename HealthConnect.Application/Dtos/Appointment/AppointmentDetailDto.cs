namespace HealthConnect.Application.Dtos.Appointment;

/// <summary>
/// Represents detailed information about an appointment.
/// </summary>
public record AppointmentDetailDto
{
    /// <summary>
    /// Gets the unique identifier of the appointment.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the unique identifier of the doctor for the appointment.
    /// </summary>
    public Guid DoctorId { get; init; }

    /// <summary>
    /// Gets the unique identifier of the client for the appointment.
    /// </summary>
    public Guid ClientId { get; init; }

    /// <summary>
    /// Gets the unique identifier of the availability slot for the appointment.
    /// </summary>
    public Guid AvailabilityId { get; init; }

    /// <summary>
    /// Gets the date and time of the appointment.
    /// </summary>
    public DateTime AppointmentDate { get; init; }

    /// <summary>
    /// Gets the duration of the appointment in minutes.
    /// </summary>
    public int Duration { get; init; }

    /// <summary>
    /// Gets the status of the appointment.
    /// </summary>
    required public string Status { get; init; }

    /// <summary>
    /// Gets any notes associated with the appointment.
    /// </summary>
    required public string Notes { get; init; }

    /// <summary>
    /// Gets the name of the doctor for the appointment.
    /// </summary>
    required public string DoctorName { get; init; }

    /// <summary>
    /// Gets the name of the client for the appointment.
    /// </summary>
    required public string ClientName { get; init; }
}
