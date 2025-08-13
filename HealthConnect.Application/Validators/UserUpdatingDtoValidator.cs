using FluentValidation;
using FluentValidation.Validators;
using HealthConnect.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Application.Validators;

public class UserUpdatingDtoValidator : AbstractValidator<UserUpdatingDto>
{
    public UserUpdatingDtoValidator() 
    { 
        RuleFor(x => x.Name)
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");
        RuleFor(x => x.Password)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$").WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(254).WithMessage("Password must not exceed 100 characters.");
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required.")
            .Matches(@"^[1-9]\d{1,14}$").WithMessage("Phone must be a valid international phone number format.")
            .MaximumLength(15).WithMessage("Phone must not exceed 15 characters.");

    }


}
