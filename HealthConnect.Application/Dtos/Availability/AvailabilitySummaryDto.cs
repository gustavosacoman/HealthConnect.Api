using HealthConnect.Application.Dtos.Doctors;

namespace HealthConnect.Application.Dtos.Availability;

public record AvailabilitySummaryDto
{
    public Guid Id { get; init; }

    public Guid DoctorId { get; init; }

    public string Name { get; init; }

    public string Specialty { get; init; }

    required public IReadOnlyCollection<DoctorSpecialityDetailDto> Specialities { get; init; }

    public DateTime SlotDateTime { get; init; }

    public int DurationMinutes { get; init; }
}
