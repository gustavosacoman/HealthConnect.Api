namespace HealthConnect.Application.Validators.AvailabilityDto;

using FluentValidation;
using HealthConnect.Application.Dtos.Availability;

/// <summary>
/// Validator for <see cref="AvailabilityRegistrationDto"/>.
/// Ensures DoctorId, SlotDateTime, and DurationMinutes are valid for registration.
/// </summary>
public class AvailabilityRegistrationDtoValidator : AbstractValidator<AvailabilityRegistrationDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AvailabilityRegistrationDtoValidator"/> class.
    /// Sets up validation rules for DoctorId, SlotDateTime, and DurationMinutes.
    /// </summary>
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
