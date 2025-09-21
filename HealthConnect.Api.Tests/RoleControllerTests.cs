using HealthConnect.Application.Dtos.Auth;
using HealthConnect.Application.Dtos.Role;
using HealthConnect.Infrastructure.Configurations;
using HealthConnect.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Api.Tests;

public class RoleControllerTests : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebAppFactory _factory;
    public RoleControllerTests(CustomWebAppFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();

        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.
                Add(new System.Net.Http.Headers
                .MediaTypeWithQualityHeaderValue("application/json"));


            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            SeedData.PopulateDatabase(db, new CryptoHelper());
        }
    }


    public async Task<string> AuthenticateAndGetTokenAsync()
    {
        var loginRequest = new LoginRequestDto
        {
            Email = "bruno@example.com",
            Password = "Password123!",
        };
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);
        response.EnsureSuccessStatusCode();

        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

        if (loginResponse?.Token == null)
        {
            throw new InvalidOperationException("No possible to get the token");

        }

        return loginResponse.Token;
    }
    [Fact]
    public async Task GetAllRoles_ShouldReturnListOfRoles()
    {
        var token = await AuthenticateAndGetTokenAsync();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/v1/role/all");
        response.EnsureSuccessStatusCode();

        var roles = await response.Content.ReadFromJsonAsync<List<RoleSummaryDto>>();
        Assert.NotNull(roles);
        Assert.Equal("Admin", roles[0].Name);
        Assert.Contains("Patient", roles[2].Name);
        Assert.Contains("Doctor", roles[1].Name);
    }
    [Fact]
    public async Task GetRoleByName_ShouldReturnRole()
    {
        var token = await AuthenticateAndGetTokenAsync();
        
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/v1/role/Admin");

        response.EnsureSuccessStatusCode();

        var role = await response.Content.ReadFromJsonAsync<RoleSummaryDto>();

        Assert.NotNull(role);
        Assert.Equal("Admin", role.Name);
    }
    [Fact]
    public async Task GetRoleById_ShouldReturnRole()
    {
        var token = await AuthenticateAndGetTokenAsync();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/v1/role/223e4567-e89b-12d3-a456-426614174998");
        response.EnsureSuccessStatusCode();

        var role = await response.Content.ReadFromJsonAsync<RoleSummaryDto>();

        Assert.NotNull(role);
        Assert.Equal("Admin", role.Name);
    }
    [Fact]
    public async Task GetRolesForUser_ShouldReturnRoles()
    {
        var token = await AuthenticateAndGetTokenAsync();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/v1/role/user/123e4567-e89b-12d3-a456-426614174000");
        response.EnsureSuccessStatusCode();

        var roles = await response.Content.ReadFromJsonAsync<List<RoleSummaryDto>>();

        Assert.NotNull(roles);
        Assert.Equal(2, roles.Count);
        Assert.Equal("Doctor", roles[1].Name);
        Assert.Equal("Admin", roles[0].Name);
    }

    [Fact]
    public async Task CreateRole_ShouldCreateNewRole()
    {
        var token = await AuthenticateAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var newRole = new RoleRegistrationDto
        {
            Name = "Nurse"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/role", newRole);
        response.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync("/api/v1/role/Nurse");
        getResponse.EnsureSuccessStatusCode();

        var createdRole = await getResponse.Content.ReadFromJsonAsync<RoleSummaryDto>();
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(createdRole);
        Assert.Equal("Nurse", createdRole.Name);
    }
}
