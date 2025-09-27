using HealthConnect.Application.Interfaces;
using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Application.Services;
using HealthConnect.Domain.Enum;
using HealthConnect.Domain.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Application.Tests;

public class AuthServiceTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IRoleRepository> _roleRepositoryMock;
    private readonly AuthService authService;

    public AuthServiceTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _configurationMock = new Mock<IConfiguration>();
        _roleRepositoryMock = new Mock<IRoleRepository>();
        authService = new AuthService(
            _userRepositoryMock.Object,
            _passwordHasherMock.Object,
            _configurationMock.Object,
            _roleRepositoryMock.Object);
    }

    [Fact]
    public async Task LoginRequest_ShouldReturnAToken_WhenPassAValidUserAndPassoword()
    {
        var userEmail = "usertest@example.com";
        var plainPassword = "Password123@#";

        var testUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "User test",
            Email = userEmail,
            Phone = "1234567890",
            CPF = "12345678901",
            Sex = Sex.Male,
            HashedPassword = "a_real_hashed_password_from_db",
            Salt = "One Salt",
            BirthDate = new DateOnly(1990, 1, 1)
        };

        _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(userEmail))
            .ReturnsAsync(testUser);

        _passwordHasherMock.Setup(p => 
        p.VerifyPassword(plainPassword, testUser.HashedPassword, testUser.Salt))
            .Returns(true);

        _configurationMock.Setup(c => c["Jwt:Key"])
            .Returns("a-secret-and-long-enough-key-jwt-256-bits");

        var result = await authService.LoginAsync(new()
        {
            Email = "usertest@example.com",
            Password = plainPassword,
        });

        Assert.NotNull(result);

        _userRepositoryMock.Verify(r => 
        r.GetUserByEmailAsync(userEmail), Times.Once);

        _passwordHasherMock.Verify(p => 
        p.VerifyPassword(
            plainPassword,
            testUser.HashedPassword,
            testUser.Salt), Times.Once);

    }
}
