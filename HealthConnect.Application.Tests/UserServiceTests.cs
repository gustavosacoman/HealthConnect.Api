using AutoMapper;
using HealthConnect.Application.Dtos;
using HealthConnect.Application.Dtos.Users;
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
        };

        _userRepositoryMock.Setup(r => r.GetUserByEmail(command.Email))
            .ReturnsAsync(new User
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Email = command.Email,
                CPF = command.CPF,
                Phone = command.Phone,
                HashedPassword = "hashed_password",
                Salt = "salt",
                BirthDate = command.BirthDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

        await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.CreateUser(command));
    }

    [Fact]
    public async Task GetUserByEmail_ShouldReturnUserSummaryDto_WhenEmailExist()
    {
        var userEmail = "teste@example.com";
        var testUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "User teste",
            Email = userEmail,
            Phone = "1234567890",
            CPF = "12345678901",
            HashedPassword = "hashed_password",
            Salt = "One Salt",
            BirthDate = new DateOnly(1990, 1, 1)
        };
        var expectedDto = new UserSummaryDto 
        {
            Id = testUser.Id,
            Name = testUser.Name, 
            Email = testUser.Email,
            Phone = testUser.Phone,
            CPF = testUser.CPF,
            BirthDate = testUser.BirthDate
        };

        _userRepositoryMock.Setup(r => r.GetUserByEmail(userEmail))
            .ReturnsAsync(testUser);

        _mockMapper.Setup(mapper => mapper.Map<UserSummaryDto>(testUser))
        .Returns(expectedDto);

        var result = await _userService.GetUserByEmail(userEmail);

        Assert.NotNull(result);
        Assert.Equal(expectedDto.Id, result.Id);
        Assert.Equal(expectedDto.Name, result.Name);

        _userRepositoryMock.Verify(r => r.GetUserByEmail(userEmail), Times.Once);

    }

    [Fact]
    public async Task GetUserById_ShouldReturnUserSummaryDto_WhenIdExist()
    {
        var userId = Guid.NewGuid();
        var testUser = new User
        {
            Id = userId,
            Name = "User teste",
            Email = "usertest@example.com",
            Phone = "1234567890",
            CPF = "12345678901",
            HashedPassword = "hashed_password",
            Salt = "One Salt",
            BirthDate = new DateOnly(1990, 1, 1)
        };
        var expectedDto = new UserSummaryDto
        {
            Id = testUser.Id,
            Name = testUser.Name,
            Email = testUser.Email,
            Phone = testUser.Phone,
            CPF = testUser.CPF,
            BirthDate = testUser.BirthDate
        };

        _userRepositoryMock.Setup(r => r.GetUserById(userId))
            .ReturnsAsync(testUser);

        _mockMapper.Setup(mapper => mapper.Map<UserSummaryDto>(testUser))
        .Returns(expectedDto);

        var result = await _userService.GetUserById(userId);

        Assert.NotNull(result);
        Assert.Equal(expectedDto.Id, result.Id);
        Assert.Equal(expectedDto.Name, result.Name);

        _userRepositoryMock.Verify(r => r.GetUserById(userId), Times.Once);
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnListOfUserSummaryDto()
    {
        var users = new List<User>
        {
            new User 
            {
                Id = Guid.NewGuid(),
                Name = "User 1",
                Email = "teste.user1@example.com",
                Phone = "1234567890",
                CPF = "12345678901",
                BirthDate = new DateOnly(1990, 1, 1),
                HashedPassword = "hashed_password",
                Salt = "salt",
            },
            new User
            {
                Id = Guid.NewGuid(),
                Name = "User 2",
                Email = "teste.user2@example.com",
                Phone = "1234567890",
                CPF = "12345678901",
                BirthDate = new DateOnly(1990, 1, 1),
                HashedPassword = "hashed_password",
                Salt = "salt",
            },
            new User
            {
                Id = Guid.NewGuid(),
                Name = "User 3",
                Email = "teste.user3@example.com",
                Phone = "1234567890",
                CPF = "12345678901",
                BirthDate = new DateOnly(1990, 1, 1),
                HashedPassword = "hashed_password",
                Salt = "salt",
            },

        };

        _userRepositoryMock.Setup(r => r.GetAllUsers())
            .ReturnsAsync(users);

        _mockMapper.Setup(m => m.Map<IEnumerable<UserSummaryDto>>(users))
            .Returns(users.Select(u => new UserSummaryDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                CPF = u.CPF,
                BirthDate = u.BirthDate,
            }));

        var result = await _userService.GetAllUsers();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count());

    }

    [Fact]
    public async Task UpdateUser_ShouldUpdateTheUser()
    {
        var command = new UserUpdatingDto
        {
            Name = "Updated User",
            Phone = "9876543210",
            Password = "NewPassword@123",

        };

        var userId = Guid.NewGuid();
        var existingUser = new User
        {
            Id = userId,
            Name = "Old User",
            Email = "oldUser@example.com",
            HashedPassword = "old_hashed_password",
            Salt = "old_salt",
            Phone = "1234567890",
            CPF = "12345678901",
            BirthDate = new DateOnly(1990, 1, 1)
        };

        _userRepositoryMock.Setup(r => r.GetUserById(userId))
            .ReturnsAsync(existingUser);

        existingUser.Name = command.Name;
        existingUser.Phone = command.Phone;
        existingUser.HashedPassword = "new_hashed_password";

        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);

        _mockMapper.Setup(m => m.Map<UserSummaryDto>(existingUser))
            .Returns(new UserSummaryDto
            {
                Id = existingUser.Id,
                Name = existingUser.Name,
                Email = existingUser.Email,
                Phone = existingUser.Phone,
                CPF  = existingUser.CPF,
                BirthDate = existingUser.BirthDate
            });

        var result = await _userService.UpdateUser(userId, command);

        Assert.NotNull(result);
        Assert.Equal(command.Name, result.Name);
        Assert.Equal(command.Phone, result.Phone);
    }

    [Fact]
    public async Task DeleteUser_ShouldApplieSoftDeletInUser()
    {

        var userEmail = "userToDelete@example.com";
        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "User to Delete",
            Email = userEmail,
            Phone = "1234567890",
            CPF = "12345678901",
            BirthDate = new DateOnly(1990, 1, 1),
            HashedPassword = "hashed_password",
            Salt = "salt",
            DeletedAt = null
        };

        _userRepositoryMock.Setup(r => r.GetUserByEmail(userEmail))
        .ReturnsAsync(existingUser);

        await _userService.DeleteUser(userEmail);

        Assert.NotNull(existingUser.DeletedAt);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);

    }
}