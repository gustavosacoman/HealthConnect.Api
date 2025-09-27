namespace HealthConnect.Application.Dtos.Client;
public record ClientDetailDto
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public string Name { get; init; }

    public string Email { get; init; }

    public string Sex { get; init; }

    public string CPF { get; init; }

    public string Phone { get; init; }

    public DateOnly BirthDate { get; init; }

    public IReadOnlyCollection<string> Roles { get; init; }

}
