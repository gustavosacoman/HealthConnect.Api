namespace HealthConnect.Domain.Enum;

/// <summary>
/// Represents the status of an appointment.
/// </summary>
public enum AppointmentStatus
{
    /// <summary>
    /// The appointment is scheduled and has not yet occurred.
    /// </summary>
    Scheduled,

    /// <summary>
    /// The appointment has been completed.
    /// </summary>
    Completed,

    /// <summary>
    /// The appointment was cancelled by the client.
    /// </summary>
    CancelledByClient,

    /// <summary>
    /// The appointment was cancelled by the doctor.
    /// </summary>
    CancelledByDoctor,
}
