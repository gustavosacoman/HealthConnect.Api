using FluentValidation;
using HealthConnect.Application.Dtos.Availability;

namespace HealthConnect.Application.Validators.AvailabilityDto;

public class AvailabilityRegistrationDtoValidator : AbstractValidator<AvailabilityRegistrationDto>
{
    public AvailabilityRegistrationDtoValidator()
    {
        RuleFor(x => x.DoctorId)
            .NotEmpty().WithMessage("DoctorId is required.");
        RuleFor(x => x.SlotDateTime)
            .NotEmpty().WithMessage("SlotDateTime is required.")
            .Must(dateTime => dateTime > DateTime.Now).WithMessage("SlotDateTime must be in the future.");
        RuleFor(x => x.DurationMinutes)
            .NotEmpty().WithMessage("DurationMinutes is required.")
            .GreaterThan(0).WithMessage("DurationMinutes must be greater than 0.");
    }
}
