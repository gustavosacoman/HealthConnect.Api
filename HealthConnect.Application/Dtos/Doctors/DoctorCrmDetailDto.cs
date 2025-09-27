namespace HealthConnect.Application.Dtos.Doctors;

/// <summary>
/// Represents the CRM details of a doctor, including CRM number and state.
/// </summary>
public record DoctorCrmDetailDto
{
    /// <summary>
    /// Gets the CRM number of the doctor.
    /// </summary>
    required public string CrmNumber { get; init; }

    /// <summary>
    /// Gets the state associated with the CRM number.
    /// </summary>
    required public string State { get; init; }
}
