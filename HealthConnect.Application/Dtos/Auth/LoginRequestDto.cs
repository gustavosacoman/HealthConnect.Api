namespace HealthConnect.Application.Dtos.Auth;

public record LoginRequestDto
{
    required public string Email { get; init; }
    required public string Password { get; init; }
}
