namespace HealthConnect.Application.Dtos.Client;

using HealthConnect.Domain.Enum;

/// <summary>
/// Data Transfer Object for registering a new client.
/// </summary>
public class ClientRegistrationDto
{
    /// <summary>
    /// Gets the client's full name.
    /// </summary>
    required public string Name { get; init; }

    /// <summary>
    /// Gets the client's email address.
    /// </summary>
    required public string Email { get; init; }

    /// <summary>
    /// Gets the client's phone number (optional).
    /// </summary>
    public string? Phone { get; init; }

    /// <summary>
    /// Gets the client's password.
    /// </summary>
    required public string Password { get; init; }

    /// <summary>
    /// Gets the client's CPF (Cadastro de Pessoas Físicas).
    /// </summary>
    required public string CPF { get; init; }

    /// <summary>
    /// Gets the client's birth date.
    /// </summary>
    required public DateOnly BirthDate { get; init; }

    /// <summary>
    /// Gets the client's sex.
    /// </summary>
    required public Sex Sex { get; init; }
}
