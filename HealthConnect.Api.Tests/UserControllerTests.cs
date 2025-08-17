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
}
