namespace HealthConnect.Application.Dtos.Doctors;

public record DoctorRegistrationDto
{

    required public string Name { get; init; }

    required public string Email { get; init; }

    public string? Phone { get; init; }

    required public string Password { get; init; }

    required public string CPF { get; init; }

    required public DateOnly BirthDate { get; init; }

    required public string RQE { get; init; }

    required public string CRM { get; init; }

    required public string Specialty { get; init; }

    public string? Biography { get; init; }

    //public string ProfilePicture { get; init; }
}
