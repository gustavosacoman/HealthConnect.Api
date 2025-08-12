using AutoMapper;
using HealthConnect.Application.Dtos;
using HealthConnect.Application.Interfaces;
using HealthConnect.Application.Services;
using HealthConnect.Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Application.Tests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly Mock<IMapper> _mockMapper;

    private readonly UserService _userService;
    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _mockMapper = new Mock<IMapper>();


        _userService = new UserService(
            _userRepositoryMock.Object,
            _mockMapper.Object,
            _mockPasswordHasher.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task CreateUserAsync_WithValidAndUniqueData_ShouldSucceedAndSaveChanges()
    {
        var command = new UserRegistrationDto
        {
            Name = "Test User",
            Email = "teste.user@gmail.com",
            Phone = "1234567890",
            Password = "Password@123",
            CPF = "12345678901",
            BirthDate = new DateOnly(1990, 1, 1)

        };

        _userRepositoryMock.Setup(r => r.GetUserByEmail(It.IsAny<string>()))
            .ReturnsAsync((User)null);

        _userRepositoryMock.Setup(r => r.CreateUser(It.IsAny<User>()));

        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);

        _mockPasswordHasher.Setup(p => p.GenerateSalt()).Returns("fake_salt");
        _mockPasswordHasher.Setup(p => p.HashPassword(It.IsAny<string>(), It.IsAny<string>())).Returns("fake_hash");

        _mockMapper.Setup(m => m.Map<UserSummaryDto>(It.IsAny<User>()))
            .Returns(new UserSummaryDto 
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Email = command.Email,
                CPF = command.CPF,
                BirthDate = command.BirthDate,
                Phone = command.Phone
            });


        var result = await _userService.CreateUser(command);

        _userRepositoryMock.Verify(r => r.CreateUser(It.IsAny<User>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(command.Name, result.Name);
    }
}