namespace HealthConnect.Application.Dtos.Appointment;

using HealthConnect.Domain.Enum;

/// <summary>
/// Data Transfer Object for updating an appointment.
/// </summary>
public record AppointmentUpdatingDto
{
    /// <summary>
    /// Gets additional notes for the appointment.
    /// </summary>
    public string? Notes { get; init; }

    /// <summary>
    /// Gets the status of the appointment.
    /// </summary>
    public AppointmentStatus? Status { get; init; }
}