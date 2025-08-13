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

    [Fact]
    public async Task CreateUser_ShouldThrowInvalidOperationException_WhenEmailAlreadyExist()
    {
        var command = new UserRegistrationDto 
        {
            Name = "userTest",
            Email = "teste.user@gmail.com",
            Phone = "123456789",
            Password = "Password@123",
            CPF = "1345678910",
            BirthDate = new DateOnly(1990, 1, 1)
        }

        _userRepositoryMock.Setup(r => r.GetUserByEmail(command.Email))
            .ReturnsAsync(new User());

        await Assert.ThrowAsync<InvalidOperationException>(() => _userService.CreateUser(command));
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

    }

    [Fact]
    public async Task GetUserByEmail_ShouldReturnUserSummaryDto_WhenEmailExist()
    {
        var userEmail = "teste@example.com";
        var testeUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "User teste",
            Email = userEmail,
        };
        var expectedDto = new UserSummaryDto 
        {
            Id = testUser.Id,
            Name = testUser.Name, 
            Email = testUser.Email
        };

        _userRepositoryMock.Setup(r = r.GetUserByEmail(userEmail))
            .ReturnsAsync(testeUser);
        
        _mockMapper.Setup(m => m.Map<UserSummaryDto>(testeUser))
            .ReturnsAsync(expectedDto);

        var result = await _userService.GetUserByEmail(userEmail);

        Assert.NotNull(result);
        Assert.Equal(expectedDto.Id, result.Id);
        Assert.Equal(expectedDto.Name, result.Name);

        _userRepositoryMock.Verify(r => r.GetUserByEmail(userEmail), Times.Once);

    }

    [Fact]
    public async Task GetUserById_ShouldReturnUserSummaryDto_WhenIdExist()
    {
        var userId = "umGuiIdDiferente";
        var testeUser = new User 
        {
            Id = userId,
            Name = "user teste",
            Email = "teste.user@gmail.com"
        }
        var expectedDto = new UserSummaryDto 
        {
            Id = testeUser.Id,
            Name = testeUser.Name,
            Email = testeUser.Email
        }

        _userRepositoryMock.Setup(r => r.GetUserById(userId))
            .ReturnsAsync(testeUser);
        
        _mockMapper.Setup(m => m.Map<UserSummaryDto>(testeUser))
            .ReturnsAsync(expectedDto);

        var result = await _userService.GetUserById(userId);

        Assert.NotNull(result);
        Assert.Equal(expectedDto.Id, result.Id);
        Assert.Equal(expectedDto.Name, result.Name);

        _userRepositoryMock.Verify(r => r.GetUserByEmail(userEmail), Times.Once);
    }
}