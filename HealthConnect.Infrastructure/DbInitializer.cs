using HealthConnect.Application.Dtos.Client;
using HealthConnect.Application.Dtos.Doctors;
using HealthConnect.Application.Dtos.Speciality;
using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Models;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        var specialityService = scope.ServiceProvider.GetRequiredService<ISpecialityService>();

        var doctorEmail = "john@admin.com";
        var clientEmail = "julia@admin.com";
        var maryEmail = "mary@admin.com";
        var davidEmail = "david@admin.com";

        var names = new List<string>
        {
            "Cardiologista",
            "Dermatologista",
            "Neurologista",
            "Pediatra",
            "Psiquiatra",
            "Ortopedista",
            "Ginecologista",
            "Oftalmologista",
            "Endocrinologista",
            "Urologista",
            "Otorrinolaringologista",
        };

        foreach (var name in names)
        {
            var specialits = await specialityService.GetSpecialityByName(name);

            if (specialits == null)
            {
                var newSpecialist = new SpecialityRegistrationDto(name);

                await specialityService.CreateSpeciality(newSpecialist);
                await context.SaveChangesAsync();
            }
        }

        var cardiologySpecialty = await specialityService.GetSpecialityByName("Cardiologista");
        var dermatologySpecialty = await specialityService.GetSpecialityByName("Dermatologista");

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
                SpecialityId = cardiologySpecialty.Id,
                Biography = "Experienced cardiologist with over 10 years in practice.",
            };
            await userService.CreateDoctorAsync(doctor);
        }
        if (!await context.Users.AnyAsync(u => u.Email == maryEmail))
        {
            var doctor = new DoctorRegistrationDto
            {
                Name = "Dr. Mary",
                Email = "mary@admin.com",
                Password = "Admin@123",
                CPF = "12345678950",
                BirthDate = new DateOnly(1980, 1, 1),
                RQE = "RQE12369",
                CRM = "CRM67823",
                SpecialityId = cardiologySpecialty.Id,
                Biography = "Experienced cardiologist with over 10 years in practice.",
            };
            await userService.CreateDoctorAsync(doctor);
        }
        if (!await context.Users.AnyAsync(u => u.Email == davidEmail))
        {
            var doctor = new DoctorRegistrationDto
            {
                Name = "Dr. David ",
                Email = "david@admin.com",
                Password = "Admin@123",
                CPF = "12345678980",
                BirthDate = new DateOnly(1980, 1, 1),
                RQE = "RQE12387",
                CRM = "CRM67882",
                SpecialityId = cardiologySpecialty.Id,
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