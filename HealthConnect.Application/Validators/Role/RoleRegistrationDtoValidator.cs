namespace HealthConnect.Application.Validators.Role;

using FluentValidation;
using HealthConnect.Application.Dtos.Role;

/// <summary>
/// Validator for <see cref="RoleRegistrationDto"/>.
/// Ensures that the role name is provided and does not exceed 50 characters.
/// </summary>
public class RoleRegistrationDtoValidator : AbstractValidator<RoleRegistrationDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RoleRegistrationDtoValidator"/> class.
    /// </summary>
    public RoleRegistrationDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Role name is required.")
            .MaximumLength(50).WithMessage("Role name must not exceed 50 characters.");
    }
}
