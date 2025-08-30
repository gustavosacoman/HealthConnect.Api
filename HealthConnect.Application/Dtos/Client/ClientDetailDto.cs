namespace HealthConnect.Application.Dtos.Client;
public record ClientDetailDto
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public string Name { get; init; }

    public string Email { get; init; }

    public string Cpf { get; init; }

    public DateTime BirthDate { get; init; }

    public string Phone { get; init; }

}
