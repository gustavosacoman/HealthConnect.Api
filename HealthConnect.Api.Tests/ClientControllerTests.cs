using HealthConnect.Application.Dtos.Auth;
using HealthConnect.Application.Dtos.Client;
using HealthConnect.Infrastructure.Configurations;
using HealthConnect.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Json;
using System.Numerics;
using System.Xml.Linq;

namespace HealthConnect.Api.Tests;

public class ClientControllerTests
    : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebAppFactory _factory;
    
    public ClientControllerTests(CustomWebAppFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();

        using (var scope = _factory.Services.CreateAsyncScope())
        {
            var scopedService = scope.ServiceProvider;
            var db = scopedService.GetRequiredService<AppDbContext>();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.
                Add(new System.Net.Http.Headers
                .MediaTypeWithQualityHeaderValue("application/json"));

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            SeedData.PopulateDatabase(db, new CryptoHelper());
        }
    }

    private async Task<string> AuthenticateAndGetTokenAsync()
    {
        var loginRequest = new LoginRequestDto
        {
            Email = "bruno@example.com",
            Password = "Password123!",
        };
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);
        response.EnsureSuccessStatusCode();

        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

        if (loginResponse?.Token is null)
        {
            throw new InvalidOperationException("No possible to get the token");

        }

        return loginResponse.Token;
    }

    [Fact]
    public async Task GetClientById_ShouldReturnAClientSummaryDto()
    {
        var token = await AuthenticateAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var id = "123e4567-e89b-12d3-a456-426614174008";

        var response = await _client.GetAsync($"/api/v1/client/{id}");
        response.EnsureSuccessStatusCode();

        var clientSummary = await response.Content.ReadFromJsonAsync<ClientSummaryDto>();

        var expectedRoles = new List<string> { "patient" };

        Assert.NotNull(clientSummary);
        Assert.Equal("Daniela Pereira", clientSummary.Name);
        Assert.Equal(id, clientSummary.Id.ToString());
    }
    [Fact]
    public async Task GetAllClients_ShouldReturnAListOfClientSummaryDto()
    {
        var token = await AuthenticateAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"/api/v1/client/all");
        response.EnsureSuccessStatusCode();

        var clients = await response.Content.ReadFromJsonAsync<List<ClientSummaryDto>>();

        Assert.NotNull(clients);
        Assert.Equal(3, clients.Count);
    }
    [Fact]
    public async Task GetClientByUserIdAsync_ShouldReturnAClientUsingTheUserId()
    {
        var token = await AuthenticateAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var userId = "123e4567-e89b-12d3-a456-426614174003";

        var response = await _client.GetAsync($"/api/v1/client/user/{userId}");
        response.EnsureSuccessStatusCode();

        var clientSummary = await response.Content.ReadFromJsonAsync<ClientDetailDto>();

        Assert.NotNull(clientSummary);
        Assert.Equal("Daniela Pereira", clientSummary.Name);
        Assert.Equal(userId, clientSummary.UserId.ToString());
    }
    [Fact]
    public async Task GetClientDetailByIdAsync_ShouldReturnAClientDetailDto()
    {
        var token = await AuthenticateAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var id = "123e4567-e89b-12d3-a456-426614174008";

        var response = await _client.GetAsync($"/api/v1/client/detail/{id}");
        response.EnsureSuccessStatusCode();

        var clientDetail = await response.Content.ReadFromJsonAsync<ClientDetailDto>();
        var expectedRoles = new List<string> { "patient" };
        Assert.NotNull(clientDetail);
        Assert.Equal(id, clientDetail.Id.ToString());
        Assert.Equal("Daniela Pereira", clientDetail.Name);
        Assert.Equal("daniela@example.com", clientDetail.Email);
        Assert.Equal("66778899000", clientDetail.CPF);
        Assert.Equal("6677889900", clientDetail.Phone);
        Assert.Equal(new DateOnly(1988, 7, 30), clientDetail.BirthDate);
    }
}
