namespace HealthConnect.Domain.Models.Specialities;

using HealthConnect.Domain.Interfaces;

/// <summary>
/// Represents a medical speciality.
/// </summary>
public class Speciality : IAuditable, ISoftDeletable
{
    /// <summary>
    /// Gets or sets the unique identifier for the speciality.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the speciality.
    /// </summary>
    required public string Name { get; set; }

    /// <summary>
    /// Gets or sets the collection of doctor specialities associated with this speciality.
    /// </summary>
    public ICollection<DoctorSpeciality>? DoctorSpecialities { get; set; }

    /// <summary>
    /// Gets or sets the description of the speciality.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the speciality was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the speciality was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the speciality was deleted, if applicable.
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}
