namespace HealthConnect.Application.Dtos.DoctorCRM;

/// <summary>
/// Represents a summary of a doctor's CRM (Conselho Regional de Medicina) information.
/// </summary>
public record DoctorCRMSummaryDto
{
    /// <summary>
    /// Gets the unique identifier for the CRM summary.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the unique identifier of the doctor.
    /// </summary>
    public Guid DoctorId { get; init; }

    /// <summary>
    /// Gets the name of the doctor.
    /// </summary>
    required public string DoctorName { get; init; }

    /// <summary>
    /// Gets the CRM number of the doctor.
    /// </summary>
    required public string CRMNumber { get; init; }

    /// <summary>
    /// Gets the state where the CRM is registered.
    /// </summary>
    required public string State { get; init; }
}
