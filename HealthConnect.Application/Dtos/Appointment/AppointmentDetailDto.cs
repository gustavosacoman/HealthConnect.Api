
namespace HealthConnect.Application.Dtos.Appointment;

public record AppointmentDetailDto
{
    public Guid Id { get; init; }

    public Guid DoctorId { get; init; }

    public Guid ClientId { get; init; }

    public Guid AvailabilityId { get; init; }

    public DateTime AppointmentDate { get; init; }

    public int Duration { get; init; }

    public string Status { get; init; }

    public string Notes { get; init; }

    public string DoctorName { get; init; }

    public string ClientName { get; init; }

}
