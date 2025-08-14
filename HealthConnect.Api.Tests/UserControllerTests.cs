using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthConnect.Api.Controllers.v1;
using HealthConnect.Application.Dtos;
using HealthConnect.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HealthConnect.Api.Tests;

public class UserControllerTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _controller = new UserController(_userServiceMock.Object);
    }

    [Fact]
    public async Task GetUserById_WhenUserExists_ShouldReturnOkResultWithUser()
    {
        var userId = Guid.NewGuid();
        var expectedUser = new UserSummaryDto
        {
            Id = userId,
            Name = "Teste User",
            Email = "testeuser@example.com"
        };

        _userServiceMock.Setup(service => service.GetUserById(userId))
                        .ReturnsAsync(expectedUser);


        var result = await _controller.GetUserById(userId);

        var okResult = Assert.IsType<OkObjectResult>(result);

        var returnedUser = Assert.IsType<UserSummaryDto>(okResult.Value);

        Assert.Equal(expectedUser.Id, returnedUser.Id);
        Assert.Equal(expectedUser.Email, returnedUser.Email);
    }

    [Fact]
    public async Task GetUserByEmail_WhenUserExists_ShouldReturnOkResultWithUser()
    {
        var email = "testeUsef@example.com";
        var expectedUser = new UserSummaryDto
        {
            Id = Guid.NewGuid(),
            Name = "Teste User",
            Email = email
        };

        _userServiceMock.Setup(service => service.GetUserByEmail(email))
                        .ReturnsAsync(expectedUser);

        var result = await _controller.GetUserByEmail(email);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUser = Assert.IsType<UserSummaryDto>(okResult.Value);

        Assert.Equal(expectedUser.Id, returnedUser.Id);
        Assert.Equal(expectedUser.Email, returnedUser.Email);
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnOkResultWithUsers()
    {
        var expectedUsers = new List<UserSummaryDto>
        {
            new UserSummaryDto { Id = Guid.NewGuid(), Name = "User 1", Email = "testeUser1@example.com" },
            new UserSummaryDto { Id = Guid.NewGuid(), Name = "User 2", Email = "testeUser2@example.com" },
            new UserSummaryDto { Id = Guid.NewGuid(), Name = "User 3", Email = "testeUser3@example.com" }
        };

        _userServiceMock.Setup(service => service.GetAllUsers())
                        .ReturnsAsync(expectedUsers);

        var result = await _controller.GetAllUsers();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUsers = Assert.IsType<List<UserSummaryDto>>(okResult.Value);

        Assert.Equal(3, returnedUsers.Count());
    }

    [Fact]
    public async Task CreateUser_ShouldReturnCreatedAtActionWithUser()
    {
        var newUser = new UserRegistrationDto
        {
            Name = "teste User",
            Email = "userTest@example.com",
            Password = "Password123@"
        };
        var createdUser = new UserSummaryDto
        {
            Id = Guid.NewGuid(),
            Name = newUser.Name,
            Email = newUser.Email
        };

        _userServiceMock.Setup(service => service.CreateUser(newUser))
                        .ReturnsAsync(createdUser);

        var result = await _controller.CreateUser(newUser);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);

        var returnedUser = Assert.IsType<UserSummaryDto>(createdAtActionResult.Value);

        Assert.Equal(createdUser.Id, returnedUser.Id);
        Assert.Equal(nameof(UserController.GetUserById), createdAtActionResult.ActionName);

    }

    [Fact]
    public async Task DeleteUser_WhenUserExists_ShouldReturnNoContent()
    {
        var userEmail = "userTest@example.com";

        var result = await _controller.DeleteUser(userEmail);

        Assert.IsType<NoContentResult>(result);

        _userServiceMock.Verify(service => service.DeleteUser(userEmail), Times.Once);

    }

}
