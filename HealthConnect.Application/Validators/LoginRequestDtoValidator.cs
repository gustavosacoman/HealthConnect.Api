namespace HealthConnect.Application.Validators;

using FluentValidation;
using HealthConnect.Application.Dtos.Auth;

/// <summary>
/// Validator for <see cref="LoginRequestDto"/>.
/// </summary>
public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginRequestDtoValidator"/> class.
    /// Defines validation rules for <see cref="LoginRequestDto"/>.
    /// </summary>
    public LoginRequestDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Invalid email format.")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
