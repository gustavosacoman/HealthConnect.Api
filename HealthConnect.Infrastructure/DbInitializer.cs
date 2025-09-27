using HealthConnect.Application.Dtos.Client;
using HealthConnect.Application.Dtos.Doctors;
using HealthConnect.Application.Dtos.Role;
using HealthConnect.Application.Dtos.Speciality;
using HealthConnect.Application.Dtos.Users;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Enum;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides methods to initialize the database with default data such as roles, specialities, and users.
/// </summary>
public static class DbInitializer
{
    /// <summary>
    /// Initializes the database with default roles, specialities, and users if they do not exist.
    /// </summary>
    /// <param name="serviceProvider">The service provider for dependency injection.</param>
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        var specialityService = scope.ServiceProvider.GetRequiredService<ISpecialityService>();
        var roleService = scope.ServiceProvider.GetRequiredService<IRoleService>();

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

        var rolesList = new List<RoleRegistrationDto>
            {
                new RoleRegistrationDto { Name = "admin" },
                new RoleRegistrationDto { Name = "doctor" },
                new RoleRegistrationDto { Name = "patient" },
            };

        foreach (var role in rolesList)
        {
            if (!await context.Roles.AnyAsync(r => r.Name == role.Name))
            {
                await roleService.CreateRoleAsync(role);
            }
        }

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
                Phone = "12345678901",
                CRM = "000001",
                CRMState = "PR",
                Sex = Sex.Male,
                Speciality = cardiologySpecialty.Name,
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
                Phone = "12345678902",
                CRM = "000000",
                CRMState = "PR",
                Sex = Sex.Female,
                Speciality = cardiologySpecialty.Name,
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
                Phone = "12345678903",
                CRM = "000002",
                CRMState = "PR",
                Sex = Sex.Male,
                Speciality = cardiologySpecialty.Name,
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
                Sex = Sex.Female,
                BirthDate = new DateOnly(1990, 5, 15),
                Phone = "12345678901",
            };
            await userService.CreateClientAsync(client);
        }

        var user = await userService.GetUserByEmailAsync(doctorEmail);
        if (user != null && !await context.UserRoles.AnyAsync(ur => ur.UserId == user.Id && ur.Role.Name == "admin"))
        {
            var adminRole = await roleService.GetRoleByNameAsync("admin");
            if (adminRole != null)
            {
                await userService.AddRoleLinkToUserAsync(new UserRoleRequestDto { Email = doctorEmail, RoleName = "admin" });
            }
        }
    }
}
