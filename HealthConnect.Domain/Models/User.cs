namespace HealthConnect.Domain.Models;

using HealthConnect.Domain.Interfaces;

/// <summary>
/// Represents a user in the HealthConnect system.
/// </summary>
public class User : IAuditable, ISoftDeletable
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    required public string Name { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    required public string Email { get; set; }

    /// <summary>
    /// Gets or sets the CPF (Cadastro de Pessoas Físicas) of the user.
    /// </summary>
    required public string CPF { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the user.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Gets or sets the hashed password of the user.
    /// </summary>
    required public string HashedPassword { get; set; }

    /// <summary>
    /// Gets or sets the salt used for password hashing.
    /// </summary>
    required public string Salt { get; set; }

    /// <summary>
    /// Gets or sets the associated doctor information for the user, if applicable.
    /// </summary>
    public Doctor? Doctor { get; set; }

    /// <summary>
    /// Gets or sets the birth date of the user.
    /// </summary>
    public DateOnly BirthDate { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user was deleted, if applicable.
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}
