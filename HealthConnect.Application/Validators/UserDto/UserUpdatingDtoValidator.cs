namespace HealthConnect.Application.Validators.UserDto;

using FluentValidation;
using HealthConnect.Application.Dtos.Users;

/// <summary>
/// Validator for <see cref="UserUpdatingDto"/>. Ensures that user update data meets required validation rules.
/// </summary>
public class UserUpdatingDtoValidator : AbstractValidator<UserUpdatingDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserUpdatingDtoValidator"/> class.
    /// </summary>
    public UserUpdatingDtoValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");
        RuleFor(x => x.Password)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$").WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(254).WithMessage("Password must not exceed 100 characters.");
        RuleFor(x => x.Phone)
            .Matches(@"^[1-9]\d{1,14}$").WithMessage("Phone must be a valid international phone number format.")
            .MaximumLength(15).WithMessage("Phone must not exceed 15 characters.");
    }
}
