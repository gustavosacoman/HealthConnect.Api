using FluentValidation;
using HealthConnect.Application.Dtos.Role;

namespace HealthConnect.Application.Validators.Role;

public class RoleRegistrationDtoValidator : AbstractValidator<RoleRegistrationDto>
{
    public RoleRegistrationDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Role name is required.")
            .MaximumLength(50).WithMessage("Role name must not exceed 50 characters.");
    }
}
