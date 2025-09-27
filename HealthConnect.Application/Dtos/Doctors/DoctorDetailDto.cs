namespace HealthConnect.Application.Dtos.Doctors;

/// <summary>
/// Data Transfer Object containing detailed information about a doctor.
/// </summary>
public class DoctorDetailDto
{
    /// <summary>
    /// Gets the unique identifier of the doctor.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the collection of the doctor's specialities.
    /// </summary>
    required public IReadOnlyCollection<DoctorSpecialityDetailDto> Specialities { get; init; }

    /// <summary>
    /// Gets the collection of the doctor's CRMs (Regional Medical Council registrations).
    /// </summary>
    required public IReadOnlyCollection<DoctorCrmDetailDto> CRMs { get; init; }

    /// <summary>
    /// Gets the sex of the doctor.
    /// </summary>
    required public string Sex { get; init; }

    /// <summary>
    /// Gets the biography of the doctor.
    /// </summary>
    public string? Biography { get; init; }

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
