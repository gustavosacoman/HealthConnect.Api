namespace HealthConnect.Application.Dtos.Availability;

public record AvailabilityRegistrationDto
{
    required public Guid DoctorId { get; init; }
    required public DateTime SlotDateTime { get; init; }
    required public int DurationMinutes { get; init; }
}
