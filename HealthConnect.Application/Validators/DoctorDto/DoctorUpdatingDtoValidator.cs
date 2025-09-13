namespace HealthConnect.Application.Validators.DoctorDto;

using FluentValidation;
using HealthConnect.Application.Dtos.Doctors;

public class DoctorUpdatingDtoValidator : AbstractValidator<DoctorUpdatingDto>
{
    public DoctorUpdatingDtoValidator()
    {
        RuleFor(x => x.CRM)
            .MaximumLength(20).WithMessage("CRM must not exceed 20 characters.");
        RuleFor(x => x.Biography)
            .MaximumLength(3500).WithMessage("Biography must not exceed 1000 characters.");
        RuleFor(x => x.RQE)
            .MaximumLength(20).WithMessage("RQE must not exceed 20 characters.");
    }
}
