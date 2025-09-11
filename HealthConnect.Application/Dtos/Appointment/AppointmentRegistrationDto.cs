namespace HealthConnect.Application.Dtos.Appointment;

public record AppointmentRegistrationDto
{
    public Guid AvailabilityId { get; init; }

    public string? Notes { get; init; }
}
