using FluentValidation;
using HealthConnect.Application.Dtos.Appointment;

namespace HealthConnect.Application.Validators.AppointmentDto;

public class AppointmentRegistrationDtoValidator : AbstractValidator<AppointmentRegistrationDto>
{
    public AppointmentRegistrationDtoValidator()
    {
        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes must not exceed 1000 characters.");
        RuleFor(x => x.AvailabilityId)
            .NotEmpty().WithMessage("AvailabilityId is required");
    }
}
