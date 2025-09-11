using HealthConnect.Domain.Enum;

namespace HealthConnect.Application.Dtos.Appointment;

public record AppointmentUpdatingDto
{
    public string? Notes { get; init; }
    public AppointmentStatus? Status { get; init; }

}