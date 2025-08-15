namespace HealthConnect.Application.Dtos;

/// <summary>
/// Data Transfer Object for user registration.
/// </summary>
public record UserRegistrationDto
{
    /// <summary>
    /// Gets the user's full name.
    /// </summary>
    required public string Name { get; init; }

    /// <summary>
    /// Gets the user's email address.
    /// </summary>
    required public string Email { get; init; }

    /// <summary>
    /// Gets the user's phone number.
    /// </summary>
    required public string Phone { get; init; }

    /// <summary>
    /// Gets the user's password.
    /// </summary>
    required public string Password { get; init; }

    /// <summary>
    /// Gets the user's CPF (Cadastro de Pessoas Físicas).
    /// </summary>
    required public string CPF { get; init; }

    /// <summary>
    /// Gets the user's birth date.
    /// </summary>
    required public DateOnly BirthDate { get; init; }
}
