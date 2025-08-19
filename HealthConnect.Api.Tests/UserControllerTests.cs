using HealthConnect.Api;
using HealthConnect.Api.Tests;
using HealthConnect.Application.Dtos;
using HealthConnect.Application.Dtos.Users;
using HealthConnect.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace HealthConnect.Api.Tests;
public class UserControllerTests
    : IClassFixture<CustomWebAppFactory>

{
    private readonly HttpClient _client;
    private readonly CustomWebAppFactory _factory;
    public UserControllerTests(CustomWebAppFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();

        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            SeedData.PopulateDatabase(db);
        }

    }

    [Fact]
    public async Task GetAllUsers_WhenCalled_ReturnsAllUsers()
    {
        var response = await _client.GetAsync("/api/v1/user/all");

        response.EnsureSuccessStatusCode();
        var users = await response.Content.ReadFromJsonAsync<IEnumerable<UserSummaryDto>>();

        Assert.NotNull(users);
        Assert.Equal(3, users.Count());
    }

    [Fact]
    public async Task GetUserById_WhenCalledWithValidId_ReturnsUser()
    {
        var userId = "123e4567-e89b-12d3-a456-426614174000";
        var response = await _client.GetAsync($"/api/v1/user/{userId}");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var user = await response.Content.ReadFromJsonAsync<UserSummaryDto>();
        Assert.NotNull(user);
        Assert.Equal(userId, user.Id.ToString());
        Assert.Equal(user.Name, "Bruno Costa");
    }

    [Fact]
    public async Task GetUserByEmail_WhenCalledWithValidEmail_ReturnsUserAsync()
    {
        var email = "bruno@example.com";

        var response = await _client.GetAsync($"/api/v1/user/by-email/{email}");

        response.EnsureSuccessStatusCode();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var user = await response.Content.ReadFromJsonAsync<UserSummaryDto>();
        Assert.NotNull(user);
        Assert.Equal(email, user.Email);
    }

    [Fact]
    public async Task CreateUser_ShouldCreateAUser_WhenCalled()
    {

        var newUser = new UserRegistrationDto
        {
            Name = "User test",
            Email = "userTeste@example.com",
            Password = "Password123@#",
            CPF = "58965234525",
            Phone = "1234567890",
            BirthDate = new DateOnly(1990, 1, 1),
        };
        var response = await _client.PostAsJsonAsync("/api/v1/user", newUser);

        response.EnsureSuccessStatusCode();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var user = await response.Content.ReadFromJsonAsync<UserSummaryDto>();

        Assert.NotNull(user);
        Assert.Equal(newUser.Name, user.Name);
        Assert.Equal(newUser.Email, user.Email);
    }

    [Fact]
    public async Task UpdateUser_ShouldUpdateAUser_WhenCalled()
    {
        var userId = "123e4567-e89b-12d3-a456-426614174000";

        var updatedUser = new UserUpdatingDto
        {
            Password = "NewPassword123@#",
            Phone = "2568745689",
        };
        var response = await _client.PatchAsJsonAsync($"/api/v1/user/{userId}", updatedUser);

        response.EnsureSuccessStatusCode();

        var user = await response.Content.ReadFromJsonAsync<UserSummaryDto>();
        Assert.NotNull(user);
        Assert.Equal(userId, user.Id.ToString());
        Assert.Equal(updatedUser.Phone, user.Phone);
        Assert.NotEqual("0987654321", user.Phone);
    }

    [Fact]
    public async Task SoftDeleteUser_ShouldPutADateInDeleleField_WhenCalled()
    {
        var userEmail = "bruno@example.com";
        var response = await _client.DeleteAsync($"/api/v1/user/{userEmail}");
        
        response.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync($"/api/v1/user/by-email/{userEmail}");

        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);

        var content = await getResponse.Content.ReadAsStringAsync();
        var errorResponse = JsonDocument.Parse(content).RootElement;
        Assert.Equal(404, errorResponse.GetProperty("StatusCode").GetInt32());
        Assert.Equal($"User with email {userEmail} not found.",
            errorResponse.GetProperty("Message").GetString());
    }

}
        
        
