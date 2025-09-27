namespace HealthConnect.Application.Dtos.Client;

/// <summary>
/// Represents detailed information about a client.
/// </summary>
public record ClientDetailDto
{
    /// <summary>
    /// Gets the unique identifier of the client.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the unique identifier of the associated user.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Gets the name of the client.
    /// </summary>
    required public string Name { get; init; }

    /// <summary>
    /// Gets the email address of the client.
    /// </summary>
    required public string Email { get; init; }

    /// <summary>
    /// Gets the sex of the client.
    /// </summary>
    required public string Sex { get; init; }

    /// <summary>
    /// Gets the CPF (Cadastro de Pessoas Físicas) number of the client.
    /// </summary>
    required public string CPF { get; init; }

    /// <summary>
    /// Gets the phone number of the client.
    /// </summary>
    required public string Phone { get; init; }

    /// <summary>
    /// Gets the birth date of the client.
    /// </summary>
    public DateOnly BirthDate { get; init; }
}
