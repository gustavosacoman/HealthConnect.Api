namespace HealthConnect.Application.Dtos.Speciality;

/// <summary>
/// Represents the registration information for a doctor's speciality.
/// </summary>
public record DoctorSpecialityRegistration
{
    /// <summary>
    /// Gets the RQE (Registro de Qualificação de Especialidade) number associated with the doctor's speciality.
    /// </summary>
    required public string RQENumber { get; init; }
}
