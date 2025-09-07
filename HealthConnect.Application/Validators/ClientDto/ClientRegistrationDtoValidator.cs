namespace HealthConnect.Application.Validators.ClientDto;

using FluentValidation;
using HealthConnect.Application.Dtos.Client;

public class ClientRegistrationDtoValidator : AbstractValidator<ClientRegistrationDto>
{
    public ClientRegistrationDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Invalid email format.")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");
        RuleFor(x => x.CPF)
            .NotEmpty().WithMessage("CPF is required.")
            .Matches(@"^\d{11}$").WithMessage("CPF must contain only 11 numbers")
            .Length(11).WithMessage("CPF must contain exactly 11 characters");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$").WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(254).WithMessage("Password must not exceed 254 characters.");
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required.")
            .Matches(@"^[1-9]\d{1,14}$").WithMessage("Phone must be a valid international phone number format.")
            .MaximumLength(15).WithMessage("Phone must not exceed 15 characters.");
        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Birth date is required.")
            .LessThan(DateOnly.FromDateTime(DateTime.Now.Date)).WithMessage("Birth date must be in the past.");
    }
}
