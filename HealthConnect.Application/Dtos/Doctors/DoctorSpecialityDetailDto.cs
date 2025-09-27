namespace HealthConnect.Application.Dtos.Doctors;

/// <summary>
/// Represents the details of a doctor's speciality, including the speciality name and RQE number.
/// </summary>
public record DoctorSpecialityDetailDto
{
    /// <summary>
    /// Gets the name of the doctor's speciality.
    /// </summary>
    required public string SpecialityName { get; init; }

    /// <summary>
    /// Gets the RQE (Registro de Qualificação de Especialista) number associated with the speciality.
    /// </summary>
    required public string RqeNumber { get; init; }
}
