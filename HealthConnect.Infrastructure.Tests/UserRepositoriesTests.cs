using HealthConnect.Application.Interfaces;
using HealthConnect.Domain.Enum;
using HealthConnect.Domain.Models;
using HealthConnect.Infrastructure.Data;
using HealthConnect.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Infrastructure.Tests;



public class UserRepositoriesTests : IDisposable
{

    private readonly AppDbContext _dbContext;
    private readonly UserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    private User CreateTestUser(string name, string email)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            CPF = "12345678901",
            Phone = "123456789",
            HashedPassword = "password123",
            Sex = Sex.Female,
            Salt = "randomSalt",
            BirthDate = new DateOnly(1990, 1, 1),
        };
    }

    public UserRepositoriesTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(dbContextOptions);
        _userRepository = new UserRepository(_dbContext);
        _unitOfWork = new UnitOfWork(_dbContext);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Fact]
    public async Task CreateUser_ShouldAddUserToDatabase()
    {

        var newUser = CreateTestUser("Test User", "teste.user@gmail.com");

        await _userRepository.CreateUserAsync(newUser);

        await _unitOfWork.SaveChangesAsync();

        var savedUser = await _dbContext
                                    .Users
                                    .FirstOrDefaultAsync(u =>
                                                            u.Email == newUser.Email
                                                        );

        Assert.NotNull(savedUser);
        Assert.Equal(newUser.Name, savedUser.Name);
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnAllUsers()
    {

        var user1 = CreateTestUser("User One", "user1@gmail.com");
        var user2 = CreateTestUser("User Two", "user2@gmail.com");

        await _dbContext.Users.AddRangeAsync(user1, user2);
        await _unitOfWork.SaveChangesAsync();
        IEnumerable<User> users = await _userRepository.GetAllUsersAsync();

        Assert.NotNull(users);
        Assert.Equal(2, users.Count());
    }

    [Fact]
    public async Task GetUserByEmail_ShouldReturnUser_WhenUserExist()
    {
        var user1 = CreateTestUser("User One", "user1@gmail.com");
        var user2 = CreateTestUser("User Two", "user2@gmail.com");

        await _dbContext.Users.AddRangeAsync(user1, user2);
        await _unitOfWork.SaveChangesAsync();

        var user = await _userRepository.GetUserByEmailAsync("user1@gmail.com");

        Assert.NotNull(user);
        Assert.NotEqual(user2, user);
        Assert.Equal(user1.Email, user.Email);

    }
    [Fact]
    public async Task GetUserById_ShouldReturnUser_WhenUserExist()
    {
        var user1 = CreateTestUser("User One", "user1@gmail.com");

        await _dbContext.Users.AddAsync(user1);
        await _unitOfWork.SaveChangesAsync();

        var user = await _userRepository.GetUserByIdAsync(user1.Id);

        Assert.NotNull(user);
        Assert.Equal(user1.Id, user.Id);


    }

}