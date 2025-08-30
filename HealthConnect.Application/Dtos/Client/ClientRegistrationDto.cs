namespace HealthConnect.Application.Dtos.Client;

public class ClientRegistrationDto
{
    required public string Name { get; init; }

    required public string Email { get; init; }

    public string? Phone { get; init; }

    required public string Password { get; init; }

    required public string CPF { get; init; }

    required public DateOnly BirthDate { get; init; }

}
