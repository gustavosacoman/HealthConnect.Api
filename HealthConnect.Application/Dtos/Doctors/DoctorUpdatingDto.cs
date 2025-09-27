namespace HealthConnect.Application.Dtos.Doctors;

/// <summary>
/// Data Transfer Object for updating a doctor's information.
/// </summary>
public record DoctorUpdatingDto
{
    /// <summary>
    /// Gets the RQE (Registro de Qualificação de Especialista) number of the doctor.
    /// </summary>
    public string? RQE { get; init; }

    /// <summary>
    /// Gets the CRM (Conselho Regional de Medicina) number of the doctor.
    /// </summary>
    public string? CRM { get; init; }

    /// <summary>
    /// Gets the unique identifier of the doctor's speciality.
    /// </summary>
    public Guid SpecialityId { get; init; }

    /// <summary>
    /// Gets the biography of the doctor.
    /// </summary>
    public string? Biography { get; init; }

    // public string? ProfilePicture { get; init; }
}
