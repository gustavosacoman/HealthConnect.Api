using HealthConnect.Application.Dtos.Auth;
using HealthConnect.Application.Dtos.Speciality;
using HealthConnect.Infrastructure.Configurations;
using HealthConnect.Infrastructure.Data;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace HealthConnect.Api.Tests;

public class SpecialityControllerTests : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebAppFactory _factory;
    public SpecialityControllerTests(CustomWebAppFactory factory)
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
    public async Task GetAllSpecialities_ShouldReturnListOfSpecialities()
    {

        var token = await AuthenticateAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
  
        var response = await _client.GetAsync("/api/v1/speciality/all");

        response.EnsureSuccessStatusCode();
        var specialities = await response.Content
            .ReadFromJsonAsync<IEnumerable<SpecialitySummaryDto>>();

        Assert.NotNull(specialities);
        Assert.Equal(5, specialities.Count());
    }

    [Fact]
    public async Task CreateSpeciality_ShouldReturnCreatedSpeciality()
    {
        var token = await AuthenticateAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var newSpeciality = new SpecialityRegistrationDto("Test Speciality");

        var response = await _client.PostAsJsonAsync("/api/v1/speciality", newSpeciality);
        response.EnsureSuccessStatusCode();

        var createdSpeciality = await response.Content.ReadFromJsonAsync<SpecialitySummaryDto>();

        Assert.NotNull(createdSpeciality);
        Assert.Equal(newSpeciality.Name, createdSpeciality.Name);
    }

    [Fact]
    public async Task GetSpecialityById_ShouldReturnSpecialitySummaryDto()
    {
        var token = await AuthenticateAndGetTokenAsync();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var specialityId = Guid.Parse("123e4567-e89b-12d3-a456-426614174888");

        var response = await _client.GetAsync($"/api/v1/speciality/{specialityId}");
        response.EnsureSuccessStatusCode();

        var speciality = await response.Content.ReadFromJsonAsync<SpecialitySummaryDto>();

        Assert.NotNull(speciality);
        Assert.Equal("Cardiology", speciality.Name);
    }

    [Fact]
    public async Task GetSpecialityByName_ShouldReturnSpecialitySummaryDto()
    {
        var token = await AuthenticateAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var specialityName = "Cardiology";

        var response = await _client.GetAsync($"/api/v1/speciality/by-name/{specialityName}");
        response.EnsureSuccessStatusCode();

        var speciality = await response.Content.ReadFromJsonAsync<SpecialitySummaryDto>();

        Assert.NotNull(speciality);
        Assert.Equal("Cardiology", speciality.Name);
    }
}
