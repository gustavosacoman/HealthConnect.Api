using HealthConnect.Domain.Enum;
using HealthConnect.Domain.Interfaces;

namespace HealthConnect.Domain.Models;

public class Appointment : IAuditable, ISoftDeletable
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }

    public Client Client { get; set; }

    public Guid DoctorId { get; set; }

    public Doctor Doctor { get; set; }

    public Guid AvailabilityId { get; set; }

    public Availability Availability { get; set; }

    public DateTime AppointmentDateTime { get; set; }

    public AppointmentStatus AppointmentStatus { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

}
