using HealthConnect.Application.Dtos.Auth;
using HealthConnect.Application.Dtos.DoctorCRM;
using HealthConnect.Infrastructure.Configurations;
using HealthConnect.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Api.Tests;

public class DoctorCRMControllerTests : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebAppFactory _factory;
    public DoctorCRMControllerTests(CustomWebAppFactory factory)
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
    public async Task GetAllDoctorCRMs_ShouldReturnAllDoctorsInDatabase_WhenCalled()
    {
        var token = await AuthenticateAndGetTokenAsync();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/v1/doctorcrm/all");
        response.EnsureSuccessStatusCode();

        var crms = await response.Content.ReadFromJsonAsync<IEnumerable<DoctorCRMSummaryDto>>();
        
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(crms);
        Assert.Equal(3, crms.Count());
    }
    
    [Fact]
    public async Task GetCRMByCodeAndState_ShouldReturnACRM_WhenCalledWithValidParameters()
    {
        var token = await AuthenticateAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var crmNumber = "123456";
        var state = "PR";
        
        var response = await _client.GetAsync($"/api/v1/doctorcrm/by-code?crmNumber={crmNumber}&state={state}");
        response.EnsureSuccessStatusCode();
        
        var crm = await response.Content.ReadFromJsonAsync<DoctorCRMSummaryDto>();
        
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(crm);
        Assert.Equal(crmNumber, crm.CRMNumber);
        Assert.Equal(state, crm.State);
    }
    [Fact]
    public async Task GetCRMByIdAsync_ShouldReturnACRM_WhenCalledWithValidId()
    {

        var token = await AuthenticateAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var crmId = Guid.Parse("323e4567-e89b-12d3-a456-426614174825");

        var response = await _client.GetAsync($"/api/v1/doctorcrm/{crmId}");
        response.EnsureSuccessStatusCode();

        var crm = await response.Content.ReadFromJsonAsync<DoctorCRMSummaryDto>();

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(crm);
        Assert.Equal(crmId, crm.Id);
    }
    [Fact]
    public async Task CreateCRMAsync_ShouldCreateDoctorCRM_WhenCalled()
    {
        var token = await AuthenticateAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var newCRM = new DoctorCRMRegistrationDto
        {
            DoctorId = Guid.Parse("123e4567-e89b-12d3-a456-426614174001"),
            CRMNumber = "987654",
            State = "RJ"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/doctorcrm", newCRM);
        response.EnsureSuccessStatusCode();
        var responseCRM = await _client.GetAsync($"/api/v1/doctorcrm/by-code?crmNumber={newCRM.CRMNumber}&state={newCRM.State}");
        responseCRM.EnsureSuccessStatusCode();

        var createdCRM = await responseCRM.Content.ReadFromJsonAsync<DoctorCRMSummaryDto>();

        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
        Assert.Equal(newCRM.CRMNumber, createdCRM!.CRMNumber);
        Assert.Equal(newCRM.State, createdCRM.State);
    }
}
