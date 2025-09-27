namespace HealthConnect.Application.Dtos.Client;

/// <summary>
/// Represents a summary of a client, including identification and contact details.
/// </summary>
public record ClientSummaryDto
{
    /// <summary>
    /// Gets the unique identifier of the client.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the unique identifier of the user associated with the client.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Gets the name of the client.
    /// </summary>
    required public string Name { get; init; }

    /// <summary>
    /// Gets the sex of the client.
    /// </summary>
    required public string Sex { get; init; }

    /// <summary>
    /// Gets the email address of the client.
    /// </summary>
    required public string Email { get; init; }

}
