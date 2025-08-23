using FluentValidation.TestHelper;
using HealthConnect.Application.Dtos;
using HealthConnect.Application.Dtos.Users;
using HealthConnect.Application.Validators;

namespace HealthConnect.Application.Tests;

public class UserRegistrationDtoValidatorTests
{
    private readonly UserRegistrationDtoValidator _validator;

    public UserRegistrationDtoValidatorTests()
    {
        _validator = new UserRegistrationDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Null_Or_Empty()
    {
        var dto = new UserRegistrationDto
        {
            Name = string.Empty,
            Email = "test@example.com",
            Phone = "1234567890",
            Password = "ValidPassword123@",
            CPF = "12345678901",
            BirthDate = new DateOnly(1990, 1, 1)
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(user => user.Name);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Name_Is_Specified()
    {
        var dto = new UserRegistrationDto
        {
            Name = "Valid Name",
            Email = "test@example.com",
            Phone = "1234567890",
            Password = "ValidPassword123@",
            CPF = "12345678901",
            BirthDate = new DateOnly(1990, 1, 1)
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(user => user.Name);
    }

    [Theory]
    [InlineData("email-invalido")]
    [InlineData("email@dominio")]
    [InlineData("email.com")]
    public void Should_Have_Error_When_Email_Is_Invalid(string invalidEmail)
    {
        var dto = new UserRegistrationDto
        {
            Name = "Valid Name",
            Password = "ValidPassword123@",
            CPF = "12345678901",
            Phone = "1234567890",
            BirthDate = new DateOnly(1990, 1, 1),
            Email = invalidEmail
        };

        var result = _validator.TestValidate(dto);


        result.ShouldHaveValidationErrorFor(user => user.Email);
    }
}
