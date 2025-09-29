using HealthConnect.Application.Dtos.Auth;
using HealthConnect.Application.Dtos.DoctorOffice;
using HealthConnect.Infrastructure.Configurations;
using HealthConnect.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace HealthConnect.Api.Tests;

public class DoctorOfficeControllerTests : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebAppFactory _factory;
    public DoctorOfficeControllerTests(CustomWebAppFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();

        using (var scope = _factory.Services.CreateScope())
        {
            var scopetedService = scope.ServiceProvider;
            var db = scopetedService.GetRequiredService<AppDbContext>();
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

        if (loginResponse?.Token == null)
        {
            throw new InvalidOperationException("No possible to get the token");

        }

        return loginResponse.Token;
    }

    [Fact]
    public async Task GetOfficeById_ShouldReturnAnOffice_WhenCalled()
    {
        var token = await AuthenticateAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var officeId = Guid.Parse("423e4567-e89b-12d3-a456-426614174555");

        var response = await _client.GetAsync($"/api/v1/doctoroffice/{officeId}");

        response.EnsureSuccessStatusCode();

        var office = await response.Content.ReadFromJsonAsync<DoctorOfficeSummaryDto>();

        Assert.NotNull(office);
        Assert.Equal(officeId, office.Id);
    }

    [Fact]
    public async Task GetOfficesByDoctoId_ShouldReturnAListOfOffices_WhenCalled()
    {
        var token = await AuthenticateAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var doctorId = Guid.Parse("123e4567-e89b-12d3-a456-426614174001");

        var response = await _client.GetAsync($"/api/v1/doctoroffice/all/by-doctor/{doctorId}");
        response.EnsureSuccessStatusCode();

        var offices = await response.Content.ReadFromJsonAsync<List<DoctorOfficeSummaryDto>>();

        Assert.NotNull(offices);
        Assert.NotEmpty(offices);
        Assert.Equal(2, offices.Count());
    }

    [Fact]
    public async Task GetAllDoctorOffices_ShouldReturnAListOfOffices_WhenCalled()
    {
        var token = await AuthenticateAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"/api/v1/doctoroffice/all");
        response.EnsureSuccessStatusCode();

        var offices = await response.Content.ReadFromJsonAsync<List<DoctorOfficeSummaryDto>>();

        Assert.NotNull(offices);
        Assert.NotEmpty(offices);
        Assert.Equal(2, offices.Count());
    }
    [Fact]
    public async Task CreateDoctorOffice_ShouldCreateAnOffice_WhenCalled()
    {
        var token = await AuthenticateAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var newOffice = new DoctorOfficeRegistrationDto
        {
            DoctorId = Guid.Parse("123e4567-e89b-12d3-a456-426614174001"),
            Street = "New Street",
            Number = 100,
            Complement = "Suite 200",
            Neighborhood = "Downtown",
            City = "New York",
            State = "NY",
            ZipCode = "10001",
            Phone = "555-0000",
            SecretaryPhone = "555-1111",
            SecretaryEmail = "teste@email.com",
            IsPrimary = false,
        };

        var response = await _client.PostAsJsonAsync($"/api/v1/doctoroffice", newOffice);
        response.EnsureSuccessStatusCode();

        var createdOffice = await response.Content.ReadFromJsonAsync<DoctorOfficeSummaryDto>();

        Assert.NotNull(createdOffice);
        Assert.Equal(newOffice.Street, createdOffice.Street);
        Assert.Equal(newOffice.Number, createdOffice.Number);
        Assert.Equal(newOffice.Complement, createdOffice.Complement);
        Assert.Equal(newOffice.State, createdOffice.State);
        Assert.Equal(newOffice.ZipCode, createdOffice.ZipCode);
        Assert.Equal(newOffice.Phone, createdOffice.Phone);
        Assert.Equal(newOffice.SecretaryPhone, createdOffice.SecretaryPhone);
        Assert.Equal(newOffice.SecretaryEmail, createdOffice.SecretaryEmail);
    }

    [Fact]
    public async Task GetPrimaryOfficeByDoctorId_ShouldReturnPrimaryOffice_WhenCalled()
    {
        var token = await AuthenticateAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var doctorId = Guid.Parse("123e4567-e89b-12d3-a456-426614174001");
        var response = await _client.GetAsync($"/api/v1/doctoroffice/isprimary/office/{doctorId}");
        response.EnsureSuccessStatusCode();

        var primaryOffice = await response.Content.ReadFromJsonAsync<DoctorOfficeSummaryDto>();

        Assert.NotNull(primaryOffice);
        Assert.Equal(doctorId, primaryOffice.DoctorId);
        Assert.True(primaryOffice.IsPrimary);
    }
}
