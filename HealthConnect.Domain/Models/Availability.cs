namespace HealthConnect.Domain.Models;

using HealthConnect.Domain.Interfaces;
using System;

public class Availability : IAuditable, ISoftDeletable
{
    public Guid Id { get; set; }

    required public Guid DoctorId { get; set; }

    public Doctor? Doctor { get; set; }

    required public DateTime SlotDateTime { get; set; }

    required public int DurationMinutes { get; set; }

    required public bool IsBooked { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
