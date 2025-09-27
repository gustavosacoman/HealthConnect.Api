namespace HealthConnect.Domain.Models;

using HealthConnect.Domain.Interfaces;

/// <summary>
/// Represents a doctor's CRM (Conselho Regional de Medicina) registration information.
/// </summary>
public class DoctorCRM : IAuditable, ISoftDeletable
{
    /// <summary>
    /// Gets or sets the unique identifier for the DoctorCRM.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated doctor.
    /// </summary>
    public Guid DoctorId { get; set; }

    /// <summary>
    /// Gets or sets the CRM number of the doctor.
    /// </summary>
    required public string CRMNumber { get; set; }

    /// <summary>
    /// Gets or sets the state where the CRM is registered.
    /// </summary>
    required public string State { get; set; }

    /// <summary>
    /// Gets or sets the associated doctor entity.
    /// </summary>
    required public Doctor Doctor { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the record was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the record was soft deleted, if applicable.
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}
