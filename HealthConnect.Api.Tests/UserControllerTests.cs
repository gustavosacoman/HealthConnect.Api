using HealthConnect.Api;
using HealthConnect.Application.Dtos;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
public class UserControllerTests
    : IClassFixture<CustomWebAppFactory>

{
    private readonly HttpClient _client;
    public UserControllerTests(CustomWebAppFactory factory)
    {
        _client = factory.CreateClient();
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

    public async Task SoftDeleteUser_ShouldPutADateInDeleleField_WhenCalled()
    {
        var userId = "123e4567-e89b-12d3-a456-426614174000";
        var response = await _client.DeleteAsync($"/api/v1/user/{userId}");
        
        response.EnsureSuccessStatusCode();

        var user = await _client.GetAsync($"/api/v1/user/{userId}");

        Assert.Equal(HttpStatusCode.NotFound, user.StatusCode);
        Assert.Equal("User not found", await user.Content.ReadAsStringAsync());
    }

}
        
        
