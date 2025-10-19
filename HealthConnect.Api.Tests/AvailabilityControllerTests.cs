using HealthConnect.Application.Dtos.Auth;
using HealthConnect.Application.Dtos.Availability;
using HealthConnect.Infrastructure.Configurations;
using HealthConnect.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace HealthConnect.Api.Tests;


public class AvailabilityControllerTests : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebAppFactory _factory;
    public AvailabilityControllerTests(CustomWebAppFactory factory)
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

    private async Task<string> AuthenticationAndGetToken()
    {
        var loginRequest = new LoginRequestDto
        {
            Email = "bruno@example.com",
            Password = "Password123!",
        };

        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);
        response.EnsureSuccessStatusCode();
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

        return loginResponse?.Token ?? throw new InvalidOperationException("No possible to get the token");

    }

    [Fact]
    public async Task GetAllAvailabilitiesPerDoctor_ShouldReturnAvailabilitiesThatIsWithADoctor()
    {
        var token = await AuthenticationAndGetToken();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var doctorId = "123e4567-e89b-12d3-a456-426614174001";

        var response = await _client.GetAsync($"/api/v1/availability/by-doctor/{doctorId}");
        response.EnsureSuccessStatusCode();

        var availabilities = await response.Content.ReadFromJsonAsync<List<AvailabilitySummaryDto>>();

        Assert.NotNull(availabilities);
        Assert.All(availabilities, a => Assert.Equal(doctorId, a.DoctorId.ToString()));
        Assert.Equal(3, availabilities.Count);

    }

    [Fact]
    public async Task GetAvailabilityById_ShouldReturnAnAvailability_WhenCall()
    {
        var token = await AuthenticationAndGetToken();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var availabilityId = "223e4567-e89b-12d3-a456-426614174000";
        var response = await _client.GetAsync($"/api/v1/availability/{availabilityId}");
        response.EnsureSuccessStatusCode();

        var availability = await response.Content.ReadFromJsonAsync<AvailabilitySummaryDto>();

        Assert.NotNull(availability);
        Assert.Equal(availabilityId, availability.Id.ToString());

    }
    [Fact]
    public async Task CreateAvailability_ShouldCreateAnAvailability_WhenCall()
    {
        var token = await AuthenticationAndGetToken();
        
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


        
        var newAvailability = new AvailabilityRegistrationDto
        {
            DoctorId = Guid.Parse("123e4567-e89b-12d3-a456-426614174001"),
            SlotDateTime = DateTime.UtcNow.AddDays(5).AddHours(10),
            DurationMinutes = 30
        };

        var timeToVerify = new DateTime(
            newAvailability.SlotDateTime.Year,
            newAvailability.SlotDateTime.Month,
            newAvailability.SlotDateTime.Day,
            newAvailability.SlotDateTime.Hour,
            newAvailability.SlotDateTime.Minute,
            0,
            newAvailability.SlotDateTime.Kind
            );

        var response = await _client.PostAsJsonAsync($"/api/v1/availability", newAvailability);

        response.EnsureSuccessStatusCode();

        var createdAvailability = await response.Content.ReadFromJsonAsync<AvailabilitySummaryDto>();

        Assert.NotNull(createdAvailability);
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        Assert.Equal(newAvailability.DoctorId, createdAvailability.DoctorId);
        Assert.Equal(timeToVerify, createdAvailability.SlotDateTime);
        Assert.Equal(newAvailability.DurationMinutes, createdAvailability.DurationMinutes);
    }
    [Fact]
    public async Task DeleteAvailability_ShouldDeleteAnAvailability_WhenCall()
    {
        var token = await AuthenticationAndGetToken();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var availabilityId = "223e4567-e89b-12d3-a456-426614174000";

        var response = await _client.DeleteAsync($"/api/v1/availability/{availabilityId}");
        response.EnsureSuccessStatusCode();

        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);

        var getResponse = await _client.GetAsync($"/api/v1/availability/{availabilityId}");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}
