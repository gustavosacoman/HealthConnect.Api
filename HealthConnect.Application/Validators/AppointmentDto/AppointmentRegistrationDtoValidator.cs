namespace HealthConnect.Application.Validators.AppointmentDto;

using FluentValidation;
using HealthConnect.Application.Dtos.Appointment;

/// <summary>
/// Validator for <see cref="AppointmentRegistrationDto"/>.
/// Ensures that the Notes property does not exceed 1000 characters and that AvailabilityId is provided.
/// </summary>
public class AppointmentRegistrationDtoValidator : AbstractValidator<AppointmentRegistrationDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppointmentRegistrationDtoValidator"/> class.
    /// </summary>
    public AppointmentRegistrationDtoValidator()
    {
        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes must not exceed 1000 characters.");
        RuleFor(x => x.AvailabilityId)
            .NotEmpty().WithMessage("AvailabilityId is required");
    }
}
