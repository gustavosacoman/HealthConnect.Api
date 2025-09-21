namespace HealthConnect.Application.Dtos.Users;

/// <summary>
/// Represents a summary of user information.
/// </summary>
public record UserSummaryDto
{
    /// <summary>
    /// Gets the unique identifier of the user.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the name of the user.
    /// </summary>
    required public string Name { get; init; }

    /// <summary>
    /// Gets the email address of the user.
    /// </summary>
    required public string Email { get; init; }

    /// <summary>
    /// Gets the phone number of the user.
    /// </summary>
    public string? Phone { get; init; }

    /// <summary>
    /// Gets the CPF (Cadastro de Pessoas Físicas) of the user.
    /// </summary>
    required public string CPF { get; init; }

    /// <summary>
    /// Gets the birth date of the user.
    /// </summary>
    public DateOnly BirthDate { get; init; }

    public IReadOnlyCollection<string> Roles { get; init; }
}
