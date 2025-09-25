using HealthConnect.Api;
using HealthConnect.Api.Tests;
using HealthConnect.Application.Dtos;
using HealthConnect.Application.Dtos.Auth;
using HealthConnect.Application.Dtos.Client;
using HealthConnect.Application.Dtos.DoctorCRM;
using HealthConnect.Application.Dtos.Doctors;
using HealthConnect.Application.Dtos.Speciality;
using HealthConnect.Application.Dtos.Users;
using HealthConnect.Application.Interfaces;
using HealthConnect.Infrastructure.Configurations;
using HealthConnect.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace HealthConnect.Api.Tests;
public class UserControllerTests
    : IClassFixture<CustomWebAppFactory>

{
    private readonly HttpClient _client;
    private readonly CustomWebAppFactory _factory;
    private readonly IPasswordHasher _passwordHasher;

    public UserControllerTests(CustomWebAppFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _passwordHasher = new CryptoHelper();

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

            SeedData.PopulateDatabase(db, _passwordHasher);
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
    public async Task GetAllUsers_WhenCalled_ReturnsAllUsers()
    {
        var token = await AuthenticateAndGetTokenAsync();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/v1/user/all");

        response.EnsureSuccessStatusCode();
        var users = await response.Content.ReadFromJsonAsync<IEnumerable<UserSummaryDto>>();

        Assert.NotNull(users);
        Assert.Equal(6, users.Count());
    }

    [Fact]
    public async Task GetUserById_WhenCalledWithValidId_ReturnsUser()
    {
        var token = await AuthenticateAndGetTokenAsync();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
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

        var token = await AuthenticateAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var email = "bruno@example.com";

        var response = await _client.GetAsync($"/api/v1/user/by-email/{email}");

        response.EnsureSuccessStatusCode();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var user = await response.Content.ReadFromJsonAsync<UserSummaryDto>();
        Assert.NotNull(user);
        Assert.Equal(email, user.Email);
    }

    [Fact]
    public async Task CreateDoctor_ShouldCreateAUserAndADoctor_WhenCalled()
    {
        var token = await AuthenticateAndGetTokenAsync();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var specialityId = "123e4567-e89b-12d3-a456-426614174888";

        var newUser = new DoctorRegistrationDto
        {
            Name = "User test",
            Email = "userTeste@example.com",
            Password = "Password123!@",
            CPF = "58965234525",
            Phone = "1234567890",
            BirthDate = new DateOnly(1990, 1, 1),
            RQE = "RQE123456",
            CRM = "654381",
            CRMState = "PR",
            Biography = "Experienced general practitioner with a passion for patient care.",
            SpecialityId = Guid.Parse("123e4567-e89b-12d3-a456-426614174888"),
        };

        var response = await _client.PostAsJsonAsync("/api/v1/user/doctor", newUser);
        

        var responseSpeciality = await _client.GetAsync($"/api/v1/speciality/{newUser.SpecialityId}");
        var responseCrm = await _client.GetAsync($"/api/v1/doctorcrm/by-code?crmNumber={newUser.CRM}&state={newUser.CRMState}");
        response.EnsureSuccessStatusCode();
        responseSpeciality.EnsureSuccessStatusCode();
        responseCrm.EnsureSuccessStatusCode();

        
        var doctor = await response.Content.ReadFromJsonAsync<DoctorDetailDto>();
        var speciality = await responseSpeciality.Content.ReadFromJsonAsync<SpecialitySummaryDto>();
        var crm = await responseCrm.Content.ReadFromJsonAsync<DoctorCRMSummaryDto>();

        var expectedRoles = new List<string> { "doctor" };

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(doctor);
        Assert.Equal(newUser.Name, doctor.Name);
        Assert.Equal(newUser.Email, doctor.Email);
        Assert.Equal(newUser.RQE, doctor.RQE);
        Assert.Equal(newUser.CRMState, crm.State);
        Assert.Equal(newUser.CRM, crm.CRMNumber);
        Assert.Equal(newUser.BirthDate, doctor.BirthDate);
        Assert.Equal(newUser.Phone, doctor.Phone);
        Assert.Equal(newUser.Biography, doctor.Biography);
        Assert.Equal(speciality.Name, doctor.Speciality);
        Assert.Equal(newUser.CPF, doctor.CPF);
        Assert.Equal(expectedRoles, doctor.Roles);

    }
    [Fact]
    public async Task CreateClient_ShouldCreateAUserAndAClient_WhenCalled()
    {
        var token = await AuthenticateAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var newUser = new ClientRegistrationDto
        {
            Name = "UserTest",
            Email = "userTest@example.com",
            Phone = "4112345678",
            Password = "Password123!@",
            CPF = "32165498700",
            BirthDate = new DateOnly(1995, 6, 15),
        };

        var response = await _client.PostAsJsonAsync("/api/v1/user/client", newUser);
        response.EnsureSuccessStatusCode();

        var client = await response.Content.ReadFromJsonAsync<ClientDetailDto>();

        var expectedRoles = new List<string> { "patient" };


        Assert.NotNull(client);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal(newUser.Name, client.Name);
        Assert.Equal(newUser.Email, client.Email);
        Assert.Equal(newUser.CPF, client.CPF);
        Assert.Equal(newUser.Phone, client.Phone);
        Assert.Equal(newUser.BirthDate, client.BirthDate);
        Assert.Equal(expectedRoles, client.Roles);
    }

    [Fact]
    public async Task UpdateUser_ShouldUpdateAUser_WhenCalled()
    {

        var token = await AuthenticateAndGetTokenAsync();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

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

    [Fact]
    public async Task SoftDeleteUser_ShouldPutADateInDeleleField_WhenCalled()
    {

        var token = await AuthenticateAndGetTokenAsync();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var userEmail = "carla@example.com";
        var userRQE = "RQE210987";
        var response = await _client.DeleteAsync($"/api/v1/user/{userEmail}");

        response.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync($"/api/v1/user/by-email/{userEmail}");
        var getResponseDoctor = await _client.GetAsync($"/api/v1/doctor/by-RQE/{userRQE}");

        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, getResponseDoctor.StatusCode);

        var content = await getResponse.Content.ReadAsStringAsync();
        var contentDoctor = await getResponseDoctor.Content.ReadAsStringAsync();


        var errorResponse = JsonDocument.Parse(content).RootElement;
        var errorResponseDoctor = JsonDocument.Parse(contentDoctor).RootElement;
        Assert.Equal(404, errorResponse.GetProperty("StatusCode").GetInt32());
        Assert.Equal(404, errorResponseDoctor.GetProperty("StatusCode").GetInt32());
        Assert.Equal($"User with email {userEmail} not found.",
            errorResponse.GetProperty("Message").GetString());
        Assert.Equal($"Doctor with RQE {userRQE} not found.",
            errorResponseDoctor.GetProperty("Message").GetString());

    }

    [Fact]
    public async Task AddRoleToUser_ShouldAddALinkInUserRole_WhenCalled()
    {
        var token = await AuthenticateAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var userEmail = "alice@example.com";
        var roleName = "admin";

        var userRoleRequestDto = new UserRoleRequestDto
        {
            Email = userEmail,
            RoleName = roleName,
        };

        var response = await _client.PostAsJsonAsync("/api/v1/user/add-role", userRoleRequestDto);
        response.EnsureSuccessStatusCode();

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        
        var responseUser = await _client.GetAsync($"/api/v1/user/by-email/{userEmail}");
        responseUser.EnsureSuccessStatusCode();

        var user = await responseUser.Content.ReadFromJsonAsync<UserSummaryDto>();

        var expectedRoles = new List<string> { "doctor", "admin" };
        Assert.NotNull(user);
        Assert.Equal(expectedRoles, user.Roles);


    }

    [Fact]
    public async Task RemoveUserRole_ShouldRemoveTheRole_WhenCalled()
    {
        var token = await AuthenticateAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var userEmail = "bruno@example.com";
        var roleName = "doctor";

        var userRoleRequestDto = new UserRoleRequestDto
        {
            Email = userEmail,
            RoleName = roleName,
        };

        var request = new HttpRequestMessage(HttpMethod.Delete, "/api/v1/user/remove-role")
        {
            Content = JsonContent.Create(userRoleRequestDto)
        };
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var responseUser = await _client.GetAsync($"/api/v1/user/by-email/{userEmail}");
        responseUser.EnsureSuccessStatusCode();

        var user = await responseUser.Content.ReadFromJsonAsync<UserSummaryDto>();
        var expectedRoles = new List<string> { "admin" };

        Assert.NotNull(user);
        Assert.Equal(expectedRoles, user.Roles);
    }
 }
        
        
