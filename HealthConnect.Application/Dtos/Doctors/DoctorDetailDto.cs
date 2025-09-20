namespace HealthConnect.Application.Dtos.Doctors;

public class DoctorDetailDto
{
    /// <summary>
    /// Gets the unique identifier of the doctor.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the RQE of the doctor.
    /// </summary>
    required public string RQE { get; init; }

    /// <summary>
    /// Gets the CRM of the doctor.
    /// </summary>
    required public string CRM { get; init; }

    /// <summary>
    /// Gets the biography of the doctor.
    /// </summary>
    public string? Biography { get; init; }

    required public string Speciality { get; init; }

    public IReadOnlyCollection<string> DoctorRoles { get; init; }

    /// <summary>
    /// Gets the unique identifier of the user.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Gets the name of the user.
    /// </summary>
    required public string Name { get; init; }

    /// <summary>
    /// Gets the email address of the user.
    /// </summary>
    required public string Email { get; init; }

    /// <summary>
    /// Gets the phone number of the user.
    /// </summary>
    public string? Phone { get; init; }

    /// <summary>
    /// Gets the CPF (Cadastro de Pessoas Físicas) of the user.
    /// </summary>
    required public string CPF { get; init; }

    /// <summary>
    /// Gets the birth date of the user.
    /// </summary>
    public DateOnly BirthDate { get; init; }
}
