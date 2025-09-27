namespace HealthConnect.Application.Dtos.DoctorCRM;

/// <summary>
/// Data transfer object for registering a doctor's CRM information.
/// </summary>
public record DoctorCRMRegistrationDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the doctor.
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
}
