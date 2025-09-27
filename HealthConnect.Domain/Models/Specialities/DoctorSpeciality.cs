namespace HealthConnect.Domain.Models.Specialities;

using HealthConnect.Domain.Interfaces;

/// <summary>
/// Represents the association between a doctor and a speciality, including RQE number and audit information.
/// </summary>
public class DoctorSpeciality : IAuditable, ISoftDeletable
{
    /// <summary>
    /// Gets or sets the unique identifier of the doctor.
    /// </summary>
    public Guid DoctorId { get; set; }

    /// <summary>
    /// Gets or sets the doctor entity.
    /// </summary>
    required public Doctor Doctor { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the speciality.
    /// </summary>
    public Guid SpecialityId { get; set; }

    /// <summary>
    /// Gets or sets the speciality entity.
    /// </summary>
    required public Speciality Speciality { get; set; }

    /// <summary>
    /// Gets or sets the RQE (Registro de Qualificação de Especialista) number for the doctor in this speciality.
    /// </summary>
    required public string RqeNumber { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the record was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the record was deleted, if applicable.
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}
