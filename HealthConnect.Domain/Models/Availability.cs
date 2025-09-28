namespace HealthConnect.Domain.Models;

using HealthConnect.Domain.Interfaces;
using System;

/// <summary>
/// Represents a doctor's availability slot for appointments.
/// </summary>
public class Availability : IAuditable, ISoftDeletable
{
    /// <summary>
    /// Gets or sets the unique identifier for the availability slot.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the doctor associated with this availability.
    /// </summary>
    required public Guid DoctorId { get; set; }

    /// <summary>
    /// Gets or sets the doctor associated with this availability.
    /// </summary>
    public Doctor? Doctor { get; set; }

    /// <summary>
    /// Gets or sets the doctor office associated with this availability.
    /// </summary>
    public DoctorOffice? DoctorOffice { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the doctor office associated with this availability.
    /// </summary>
    public Guid? DoctorOfficeId { get; set; }

    /// <summary>
    /// Gets or sets the date and time of the availability slot.
    /// </summary>
    required public DateTime SlotDateTime { get; set; }

    /// <summary>
    /// Gets or sets the duration of the availability slot in minutes.
    /// </summary>
    required public int DurationMinutes { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the slot is booked.
    /// </summary>
    required public bool IsBooked { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the availability was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the availability was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the availability was deleted, if applicable.
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}
