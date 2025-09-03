namespace HealthConnect.Application.Dtos.Availability;

public record AvailabilityRegistrationDto
{
    public Guid DoctorId { get; init; }
    public DateTime SlotDateTime { get; init; }
    public int DurationMinutes { get; init; }
}
