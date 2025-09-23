using AutoMapper;
using HealthConnect.Application.Dtos.Users;
using HealthConnect.Application.Interfaces;
using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Application.Services;
using HealthConnect.Domain.Models;
using Moq;

namespace HealthConnect.Application.Tests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IDoctorRepository> _doctorRepository;
    private readonly Mock<IClientRepository> _clientMockRepository;
    private readonly Mock<ISpecialityRepository> _specialityMockRepository;
    private readonly Mock<IRoleRepository> _roleMockRepository;
    private readonly Mock<IDoctorCRMRepository> _doctorCRMMockRepository;

    private readonly UserService _userService;
    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _doctorRepository = new Mock<IDoctorRepository>();
        _mockMapper = new Mock<IMapper>();
        _clientMockRepository = new Mock<IClientRepository>();
        _specialityMockRepository = new Mock<ISpecialityRepository>();
        _roleMockRepository = new Mock<IRoleRepository>();
        _doctorCRMMockRepository = new Mock<IDoctorCRMRepository>();


        _userService = new UserService(
            _userRepositoryMock.Object,
            _mockMapper.Object,
            _mockPasswordHasher.Object,
            _unitOfWorkMock.Object,
            _doctorRepository.Object,
            _clientMockRepository.Object,
            _specialityMockRepository.Object,
            _roleMockRepository.Object,
            _doctorCRMMockRepository.Object
        );
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

        _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(userEmail))
            .ReturnsAsync(testUser);

        _mockMapper.Setup(mapper => mapper.Map<UserSummaryDto>(testUser))
        .Returns(expectedDto);

        var result = await _userService.GetUserByEmailAsync(userEmail);

        Assert.NotNull(result);
        Assert.Equal(expectedDto.Id, result.Id);
        Assert.Equal(expectedDto.Name, result.Name);

        _userRepositoryMock.Verify(r => r.GetUserByEmailAsync(userEmail), Times.Once);

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

        _userRepositoryMock.Setup(r => r.GetUserByIdAsync(userId))
            .ReturnsAsync(testUser);

        _mockMapper.Setup(mapper => mapper.Map<UserSummaryDto>(testUser))
        .Returns(expectedDto);

        var result = await _userService.GetUserByIdAsync(userId);

        Assert.NotNull(result);
        Assert.Equal(expectedDto.Id, result.Id);
        Assert.Equal(expectedDto.Name, result.Name);

        _userRepositoryMock.Verify(r => r.GetUserByIdAsync(userId), Times.Once);
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

        _userRepositoryMock.Setup(r => r.GetAllUsersAsync())
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

        var result = await _userService.GetAllUsersAsync();

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

        _userRepositoryMock.Setup(r => r.GetUserByIdAsync(userId))
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

        var result = await _userService.UpdateUserAsync(userId, command);

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
            Id = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
            Name = "User to Delete",
            Email = userEmail,
            Phone = "1234567890",
            CPF = "12345678901",
            BirthDate = new DateOnly(1990, 1, 1),
            HashedPassword = "hashed_password",
            Salt = "salt",
            DeletedAt = null,


        };

        var doctor = new Doctor
        {
            Id = Guid.NewGuid(),
            RQE = "RQE654321",
            Biography = "Experienced general practitioner with a passion for patient care.",
            SpecialityId = Guid.NewGuid(),
            UserId = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
            User = existingUser,
            CreatedAt = DateTime.UtcNow,
            DeletedAt = null,
        };

        existingUser.Doctor = doctor;

        _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(userEmail))
        .ReturnsAsync(existingUser);

        await _userService.DeleteUserAsync(userEmail);
        Assert.NotNull(existingUser.Doctor.DeletedAt);

        Assert.NotNull(existingUser.DeletedAt);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);

    }
}