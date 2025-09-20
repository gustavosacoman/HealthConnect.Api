namespace HealthConnect.Application.Dtos.Client;

public record ClientSummaryDto
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public string Name { get; init; }

    public string Email { get; init; }

    public IReadOnlyCollection<string> PatientRoles { get; init; }
}
