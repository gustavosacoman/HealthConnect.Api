using HealthConnect.Application.Dtos.Auth;
using HealthConnect.Application.Dtos.Doctors;
using HealthConnect.Infrastructure.Configurations;
using HealthConnect.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace HealthConnect.Api.Tests;

public class DoctorControllerTests : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebAppFactory _factory;

    public DoctorControllerTests(CustomWebAppFactory factory)
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
    public async Task GetDoctorById_ShouldReturnADoctorSummaryDto()
    {
        var token = await AuthenticateAndGetTokenAsync();

        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var doctorId = Guid.Parse("123e4567-e89b-12d3-a456-426614174001");

        var response = await _client.GetAsync($"/api/v1/doctor/{doctorId}");

        response.EnsureSuccessStatusCode();

        var doctorSummaryDto = await response.Content.ReadFromJsonAsync<DoctorSummaryDto>();

        var expectedRoles = new List<string> { "doctor" , "admin"};

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(doctorSummaryDto);
        Assert.Equal(doctorId, doctorSummaryDto.Id);
    }

    [Fact]
    public async Task GetDoctorByIdDetail_ShouldReturnADoctorDetailDto()
    {
        var token = await AuthenticateAndGetTokenAsync();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var doctorId = Guid.Parse("123e4567-e89b-12d3-a456-426614174001");

        var response = await _client.GetAsync($"/api/v1/doctor/detail/{doctorId}");

        response.EnsureSuccessStatusCode();

        var doctorDetail = await response.Content.ReadFromJsonAsync<DoctorDetailDto>();
        var expectedRoles = new List<string> { "doctor", "admin" };
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(doctorDetail);
        Assert.Equal(doctorId, doctorDetail.Id);

    }

    [Fact]
    public async Task GetDoctorByRQE_ShouldReturnADoctorSummaryDto()
    {
        var token = await AuthenticateAndGetTokenAsync();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var rqe = "987654";
        var response = await _client.GetAsync($"/api/v1/doctor/by-rqe/{rqe}");

        response.EnsureSuccessStatusCode();

        var doctorSummaryDto = await response.Content.ReadFromJsonAsync<DoctorSummaryDto>();
        var specialityDetail = doctorSummaryDto!.Specialities.First();

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(doctorSummaryDto);
        Assert.Equal(rqe, specialityDetail.RqeNumber);
    }

    [Fact]
    public async Task GetAllDoctors_ShouldReturnAEnumerableListOfDoctorSummaryDto()
    {
        var token = await AuthenticateAndGetTokenAsync();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"/api/v1/doctor/all");

        response.EnsureSuccessStatusCode();

        var doctors = await response.Content.ReadFromJsonAsync<IEnumerable<DoctorSummaryDto>>();

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(doctors);
        Assert.Equal(3, doctors.Count());
    }

    [Fact]
    public async Task GetDoctorsAllBySpeciality_ShouldReturnAEnumerableListOfDoctorDetailDto()
    {
        var token = await AuthenticateAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var specialityId = Guid.Parse("123e4567-e89b-12d3-a456-426614174888");

        var response = await _client.GetAsync($"/api/v1/doctor/by-Speciality/all/{specialityId}");
        response.EnsureSuccessStatusCode();

        var doctors = await response.Content.ReadFromJsonAsync<IEnumerable<DoctorDetailDto>>();

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(doctors);
        Assert.Equal(3, doctors.Count());
    }

    [Fact]
    public async Task GetDoctorDetailByUserId_ShouldReturnADoctorDetail_WhenCalled()
    {
        var token = await AuthenticateAndGetTokenAsync();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var userId = Guid.Parse("123e4567-e89b-12d3-a456-426614174000");

        var response = await _client.GetAsync($"/api/v1/doctor/detail/by-userid/{userId}");
        response.EnsureSuccessStatusCode();

        var doctor = await response.Content.ReadFromJsonAsync<DoctorDetailDto>();

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(doctor);
        Assert.Equal(userId, doctor.UserId);
    }
}
