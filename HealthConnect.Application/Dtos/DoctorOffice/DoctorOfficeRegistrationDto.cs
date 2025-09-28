namespace HealthConnect.Application.Dtos.DoctorOffice;

/// <summary>
/// Data transfer object for registering a doctor's office.
/// </summary>
public record DoctorOfficeRegistrationDto
{
    /// <summary>
    /// Gets the unique identifier of the doctor.
    /// </summary>
    public Guid DoctorId { get; init; }

    /// <summary>
    /// Gets the street address of the doctor's office.
    /// </summary>
    required public string Street { get; init; }

    /// <summary>
    /// Gets the street number of the doctor's office.
    /// </summary>
    required public int Number { get; init; }

    /// <summary>
    /// Gets the address complement, if any.
    /// </summary>
    public string? Complement { get; init; }

    /// <summary>
    /// Gets the state where the doctor's office is located.
    /// </summary>
    required public string State { get; init; }

    /// <summary>
    /// Gets the zip code of the doctor's office.
    /// </summary>
    required public string ZipCode { get; init; }

    /// <summary>
    /// Gets the phone number of the doctor's office.
    /// </summary>
    public string? Phone { get; init; }

    /// <summary>
    /// Gets the secretary's phone number, if any.
    /// </summary>
    public string? SecretaryPhone { get; init; }

    /// <summary>
    /// Gets the secretary's email address, if any.
    /// </summary>
    public string? SecretaryEmail { get; init; }

    /// <summary>
    /// Gets a value indicating whether this doctor's office is the primary office for the doctor.
    /// </summary>
    public bool IsPrimary { get; init; } = false;
}
