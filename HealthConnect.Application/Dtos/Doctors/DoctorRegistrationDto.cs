namespace HealthConnect.Application.Dtos.Doctors;

using HealthConnect.Domain.Enum;

/// <summary>
/// Data Transfer Object for registering a doctor.
/// </summary>
public record DoctorRegistrationDto
{
    /// <summary>
    /// Gets the doctor's full name.
    /// </summary>
    required public string Name { get; init; }

    /// <summary>
    /// Gets the doctor's email address.
    /// </summary>
    required public string Email { get; init; }

    /// <summary>
    /// Gets the doctor's phone number.
    /// </summary>
    public string? Phone { get; init; }

    /// <summary>
    /// Gets the doctor's password.
    /// </summary>
    required public string Password { get; init; }

    /// <summary>
    /// Gets the doctor's CPF (Cadastro de Pessoas Físicas).
    /// </summary>
    required public string CPF { get; init; }

    /// <summary>
    /// Gets the doctor's medical speciality.
    /// </summary>
    required public string Speciality { get; init; }

    /// <summary>
    /// Gets the doctor's birth date.
    /// </summary>
    required public DateOnly BirthDate { get; init; }

    /// <summary>
    /// Gets the doctor's RQE (Registro de Qualificação de Especialista).
    /// </summary>
    required public string RQE { get; init; }

    /// <summary>
    /// Gets the doctor's CRM (Conselho Regional de Medicina) number.
    /// </summary>
    required public string CRM { get; init; }

    /// <summary>
    /// Gets the state where the CRM is registered.
    /// </summary>
    required public string CRMState { get; init; }

    /// <summary>
    /// Gets the doctor's biography.
    /// </summary>
    public string? Biography { get; init; }

    /// <summary>
    /// Gets the doctor's sex.
    /// </summary>
    required public Sex Sex { get; init; }

    // public string ProfilePicture { get; init; }
}
