using HealthConnect.Application.Dtos.Appointment;
using HealthConnect.Application.Dtos.Auth;
using HealthConnect.Application.Validators;
using HealthConnect.Domain.Enum;
using HealthConnect.Domain.Models;
using HealthConnect.Infrastructure.Configurations;
using HealthConnect.Infrastructure.Data;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace HealthConnect.Api.Tests;

public class AppointmentControllerTest : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebAppFactory _factory;
    public AppointmentControllerTest(CustomWebAppFactory factory)
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

    public async Task<string> AuthenticateAndGetTokenAsync()
    {
        var loginRequest = new LoginRequest
        {
            Email = "bruno@example.com",
            Password = "Password123!",
        };
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);
        response.EnsureSuccessStatusCode();
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        return loginResponse.Token;
    }

    [Fact]
    public async Task CreateAppointment_ShouldReturnAppointmentDetail()
    {
        var token = await AuthenticateAndGetTokenAsync();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var clientId = Guid.Parse("123e4567-e89b-12d3-a456-426614174008");

        var newAppointment = new AppointmentRegistrationDto
        {
            AvailabilityId = Guid.Parse("223e4567-e89b-12d3-a456-426614174000"),
            Notes = "Regular check-up",
        };
        
        var response = await _client.PostAsJsonAsync(
            $"/api/v1/appointment/{clientId}", newAppointment);

        response.EnsureSuccessStatusCode();

        var createdAppointment = await response.Content.ReadFromJsonAsync<AppointmentDetailDto>();

        Assert.NotNull(createdAppointment);
        Assert.True(createdAppointment.Id != Guid.Empty);
        Assert.Equal(newAppointment.AvailabilityId, createdAppointment.AvailabilityId);
        Assert.Equal("Regular check-up", createdAppointment.Notes);
        Assert.Equal("Scheduled", createdAppointment.Status);
        Assert.Equal(clientId, createdAppointment.ClientId);
        
    }

    [Fact]
    public async Task GetAppointmentsByClientId_ShouldReturnAppointmentsForClient()
    {
        var token = await AuthenticateAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var clientId = Guid.Parse("123e4567-e89b-12d3-a456-426614174008");
        var response = await _client.GetAsync($"/api/v1/appointment/by-client/{clientId}");

        response.EnsureSuccessStatusCode();

        var appointments = await response.Content.ReadFromJsonAsync<IEnumerable<AppointmentDetailDto>>();

        Assert.NotNull(appointments);

        Assert.All(appointments, a => Assert.Equal(clientId, a.ClientId));

    }
    [Fact]
    public async Task GetAppointmentsByDoctorId_ShouldReturnAppointmentsForDoctor()
    {
        var token = await AuthenticateAndGetTokenAsync();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var doctorId = Guid.Parse("123e4567-e89b-12d3-a456-426614174001");

        var response = await _client.GetAsync($"/api/v1/appointment/by-doctor/{doctorId}");

        response.EnsureSuccessStatusCode();

        var appointments = await response.Content.ReadFromJsonAsync<IEnumerable<AppointmentDetailDto>>();

        Assert.NotNull(appointments);
        Assert.All(appointments, a => Assert.Equal(doctorId, a.DoctorId));
    }

    [Fact]
    public async Task GetAppointmentById_ShouldReturnAppointment()
    {
        var token = await AuthenticateAndGetTokenAsync();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var appointmentId = Guid.Parse("323e4567-e89b-12d3-a456-426614174025");

        var response = await _client.GetAsync($"/api/v1/appointment/{appointmentId}");

        response.EnsureSuccessStatusCode();

        var appointment = await response.Content.ReadFromJsonAsync<AppointmentDetailDto>();

        Assert.NotNull(appointment);
        Assert.Equal(appointmentId, appointment.Id);
    }
    [Fact]
    public async Task UpdateAppointment_ShouldReturnNoContent()
    {
        var token = await AuthenticateAndGetTokenAsync();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var appointmentId = Guid.Parse("323e4567-e89b-12d3-a456-426614174025");

        var appointmentBeforeUpdateResponse = await _client.GetAsync($"/api/v1/appointment/{appointmentId}");
        appointmentBeforeUpdateResponse.EnsureSuccessStatusCode();
        var appointmentBeforeUpdate = await appointmentBeforeUpdateResponse.Content.ReadFromJsonAsync<AppointmentDetailDto>();

        var updateDto = new AppointmentUpdatingDto
        {
            Notes = "Updated notes",
            Status = AppointmentStatus.Completed,
        };

        var response = await _client.PatchAsJsonAsync(
            $"/api/v1/appointment?Id={appointmentId}", updateDto);

        response.EnsureSuccessStatusCode();

        

        var appointmentAfterUpdateResponse = 
            await _client.GetAsync($"/api/v1/appointment/{appointmentId}");

        appointmentAfterUpdateResponse.EnsureSuccessStatusCode();
        var appointmentAfterUpdate = 
            await appointmentAfterUpdateResponse.Content.ReadFromJsonAsync<AppointmentDetailDto>();

        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        Assert.NotEqual(appointmentBeforeUpdate.Notes, appointmentAfterUpdate.Notes);
        Assert.Equal("Updated notes", appointmentAfterUpdate.Notes);
        Assert.NotEqual(appointmentBeforeUpdate.Status, appointmentAfterUpdate.Status);
    }
}
