namespace HealthConnect.Domain.Models;

using HealthConnect.Domain.Interfaces;
using HealthConnect.Domain.Models.Specialities;

/// <summary>
/// Represents a doctor entity with personal and professional information.
/// </summary>
public class Doctor : IAuditable, ISoftDeletable
{
    /// <summary>
    /// Gets or sets the unique identifier for the doctor.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the RQE (Registro de Qualificação de Especialista) number of the doctor.
    /// </summary>
    required public string RQE { get; set; }

    /// <summary>
    /// Gets or sets the biography of the doctor.
    /// </summary>
    public string? Biography { get; set; }

    /// <summary>
    /// Gets or sets the collection of doctor specialities.
    /// </summary>
    public ICollection<DoctorSpeciality>? DoctorSpecialities { get; set; }

    // public string ProfilePicture { get; set; }

    /// <summary>
    /// Gets or sets the associated user entity for the doctor.
    /// </summary>
    required public virtual User User { get; set; }

    /// <summary>
    /// Gets or sets the collection of availabilities for the doctor.
    /// </summary>
    public ICollection<Availability>? Availabilities { get; set; }

    /// <summary>
    /// Gets or sets the collection of doctor CRM records.
    /// </summary>
    public ICollection<DoctorCRM>? DoctorCRMs { get; set; }

    /// <summary>
    /// Gets or sets the user identifier associated with the doctor.
    /// </summary>
    required public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the doctor was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the doctor was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the doctor was soft deleted, if applicable.
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}
