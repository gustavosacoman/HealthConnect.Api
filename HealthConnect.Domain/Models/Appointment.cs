namespace HealthConnect.Domain.Models;

using HealthConnect.Domain.Enum;
using HealthConnect.Domain.Interfaces;

/// <summary>
/// Represents an appointment between a client and a doctor.
/// </summary>
public class Appointment : IAuditable, ISoftDeletable
{
    /// <summary>
    /// Gets or sets the unique identifier for the appointment.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the client.
    /// </summary>
    public Guid ClientId { get; set; }

    /// <summary>
    /// Gets or sets the client associated with the appointment.
    /// </summary>
    required public Client Client { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the doctor.
    /// </summary>
    public Guid DoctorId { get; set; }

    /// <summary>
    /// Gets or sets the doctor associated with the appointment.
    /// </summary>
    required public Doctor Doctor { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the availability slot.
    /// </summary>
    public Guid AvailabilityId { get; set; }

    /// <summary>
    /// Gets or sets the availability slot for the appointment.
    /// </summary>
    required public Availability Availability { get; set; }

    /// <summary>
    /// Gets or sets the date and time of the appointment.
    /// </summary>
    public DateTime AppointmentDateTime { get; set; }

    /// <summary>
    /// Gets or sets the status of the appointment.
    /// </summary>
    public AppointmentStatus AppointmentStatus { get; set; }

    /// <summary>
    /// Gets or sets additional notes for the appointment.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the appointment was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the appointment was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the appointment was deleted, if applicable.
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}
