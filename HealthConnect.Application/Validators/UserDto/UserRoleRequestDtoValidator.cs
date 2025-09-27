namespace HealthConnect.Application.Validators.UserDto;

using FluentValidation;
using HealthConnect.Application.Dtos.Users;

/// <summary>
/// Validator for <see cref="UserRoleRequestDto"/>.
/// </summary>
public class UserRoleRequestDtoValidator : AbstractValidator<UserRoleRequestDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRoleRequestDtoValidator"/> class.
    /// Defines validation rules for <see cref="UserRoleRequestDto"/>.
    /// </summary>
    public UserRoleRequestDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Invalid email format.")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");

        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("Role name is required.")
            .MaximumLength(50).WithMessage("Role name must not exceed 50 characters.");
    }
}
