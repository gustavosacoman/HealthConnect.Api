namespace HealthConnect.Domain.Models;

using HealthConnect.Domain.Interfaces;

/// <summary>
/// Represents a doctor's office, including address, contact information, and audit details.
/// </summary>
public class DoctorOffice
    : IAuditable, ISoftDeletable
{
    /// <summary>
    /// Gets or sets the unique identifier for the doctor office.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated doctor.
    /// </summary>
    public Guid DoctorId { get; set; }

    /// <summary>
    /// Gets or sets the street address of the doctor office.
    /// </summary>
    required public string Street { get; set; }

    /// <summary>
    /// Gets or sets the street number of the doctor office.
    /// </summary>
    required public int Number { get; set; }

    /// <summary>
    /// Gets or sets the complement of the office.
    /// </summary>
    public string? Complement { get; set; }

    /// <summary>
    /// Gets or sets the state where the doctor office is located.
    /// </summary>
    required public string State { get; set; }

    /// <summary>
    /// Gets or sets the zip code of the doctor office.
    /// </summary>
    required public string ZipCode { get; set; }

    /// <summary>
    /// Gets or sets the main phone number of the doctor office.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the secretary.
    /// </summary>
    public string? SecretaryPhone { get; set; }

    /// <summary>
    /// Gets or sets the email address of the secretary.
    /// </summary>
    public string? SecretaryEmail { get; set; }

    /// <summary>
    /// Gets or sets the doctor associated with this office.
    /// </summary>
    required public Doctor Doctor { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets indicates whether this office is the primary office for the doctor.
    /// </summary>
    required public bool IsPrimary { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the office was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the office was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the office was deleted, if applicable.
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    ///// <summary>
    ///// Gets or sets the collection of availabilities for this doctor office.
    ///// </summary>
    // public ICollection<Availability> Availabilities { get; set; } = new List<Availability>();
}
