namespace HealthConnect.Application.Dtos.Availability;

public record AvailabilitySummaryDto
{
    public Guid Id { get; init; }

    public Guid DoctorId { get; init; }

    public string Name { get; init; }

    public string Specialty { get; init; }

    public string RQE { get; init; }

    public DateTime SlotDateTime { get; init; }

    public int DurationMinutes { get; init; }
}
