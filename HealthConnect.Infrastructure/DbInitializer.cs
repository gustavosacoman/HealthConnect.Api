using HealthConnect.Application.Dtos.Client;
using HealthConnect.Application.Dtos.Doctors;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static System.Runtime.InteropServices.JavaScript.JSType;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.EnsureCreatedAsync();

        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

        var doctorEmail = "john@admin.com";
        var clientEmail = "julia@admin.com";

        if (!await context.Users.AnyAsync(u => u.Email == doctorEmail))
        {
            var doctor = new DoctorRegistrationDto
            {
                Name = "Dr. John Smith",
                Email = "john@admin.com",
                Password = "Admin@123",
                CPF = "12345678901",
                BirthDate = new DateOnly(1980, 1, 1),
                RQE = "RQE12345",
                CRM = "CRM67890",
                Specialty = "Cardiology",
                Biography = "Experienced cardiologist with over 10 years in practice.",
            };
            await userService.CreateDoctorAsync(doctor);
        }

        if (!await context.Users.AnyAsync(u => u.Email == clientEmail))
        {
            var client = new ClientRegistrationDto
            {
                Name = "Julia",
                Email = "julia@admin.com",
                Password = "Client@123",
                CPF = "10987654321",
                BirthDate = new DateOnly(1990, 5, 15),
                Phone = "12345678901",
            };
            await userService.CreateClientAsync(client);
        }
    }
}