using HealthConnect.Application.Interfaces;
using HealthConnect.Domain.Models;
using HealthConnect.Domain.Models.Roles;
using HealthConnect.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Api.Tests;

public static class SeedData
{
    public const string DefaultTestUserPassword = "Password123!";

    public static void PopulateDatabase(AppDbContext context, IPasswordHasher passwordHasher)
    {
        var salt = passwordHasher.GenerateSalt();
        var hashedPassword = passwordHasher.HashPassword(DefaultTestUserPassword, salt);

        var specialities = new List<Speciality>
        {
            new Speciality { Id = Guid.Parse("123e4567-e89b-12d3-a456-426614174888"), Name = "Cardiology" },
            new Speciality { Id = Guid.NewGuid(), Name = "Dermatology" },
            new Speciality { Id = Guid.NewGuid(), Name = "Neurology" },
            new Speciality { Id = Guid.NewGuid(), Name = "Pediatrics" },
            new Speciality { Id = Guid.NewGuid(), Name = "Psychiatry" }
        };

        var Roles = new List<Role>
        {
            new Role { Id = Guid.Parse("223e4567-e89b-12d3-a456-426614174998"), Name = "admin" },
            new Role { Id = Guid.Parse("323e4567-e89b-12d3-a456-426614174997"), Name = "doctor" },
            new Role { Id = Guid.Parse("423e4567-e89b-12d3-a456-426614174999"), Name = "patient" }
        };

        var users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                Name = "Alice Silva",
                Email = "alice@example.com",
                HashedPassword = "hashed_password_1",
                Salt = "a random_salt_value",
                CPF = "12345678901",
                Phone = "1234567890",
                BirthDate = new DateOnly(1990, 1, 1)
            },
            new User
            {
                Id = Guid.Parse("123e4567-e89b-12d3-a456-426614174000"),
                Name = "Bruno Costa",
                Email = "bruno@example.com",
                HashedPassword = hashedPassword,
                Salt = salt,
                CPF = "10987654321",
                Phone = "0987654321",
                BirthDate = new DateOnly(1985, 5, 15)
            },
            new User
            {
                Id = Guid.NewGuid(),
                Name = "Carla Dias",
                Email = "carla@example.com",
                HashedPassword = "hashed_password_3",
                Salt = "yet another random_salt_value",
                CPF = "11223344556",
                Phone = "1122334455",
                BirthDate = new DateOnly(1992, 3, 20)
            },
            new User
            {
                Id = Guid.Parse("123e4567-e89b-12d3-a456-426614174003"),
                Name = "Daniela Pereira",
                Email = "daniela@example.com",
                HashedPassword = "hashed_password_4",
                Salt = "some_random_salt_value",
                CPF = "66778899000",
                Phone = "6677889900",
                BirthDate = new DateOnly(1988, 7, 30)
            },
            new User
            {
                Id = Guid.NewGuid(),
                Name = "Eduardo Gomes",
                Email = "eduardo@example.com",
                HashedPassword = "hashed_password_5",
                Salt = "different_random_salt_value",
                CPF = "44556677889",
                Phone = "4455667788",
                BirthDate = new DateOnly(1995, 11, 25)
            },
            new User
            {
                Id = Guid.NewGuid(),
                Name = "Fernanda Lima",
                Email = "fernanda@example.com",
                HashedPassword = "hashed_password_6",
                Salt = "unique_random_salt_value",
                CPF = "99887766554",
                Phone = "9988776655",
                BirthDate = new DateOnly(1991, 9, 10)
            }
        };

        var doctors = new List<Doctor>
        {
            new Doctor
            {
                Id = Guid.NewGuid(),
                UserId = users[0].Id,
                CRM = "CRM123456",
                RQE = "RQE654321",
                Speciality = specialities[0],
                SpecialityId = specialities[0].Id,
                User = users[0]
            },
            new Doctor
            {
                Id = Guid.Parse("123e4567-e89b-12d3-a456-426614174001"),
                UserId = users[1].Id,
                CRM = "CRM654321",
                RQE = "RQE987654",
                Speciality = specialities[0],
                SpecialityId = specialities[0].Id,
                User = users[1]

            },
            new Doctor
            {
                Id = Guid.Parse("123e4567-e89b-12d3-a456-426614174010"),
                UserId = users[2].Id,
                CRM = "CRM789012",
                RQE = "RQE210987",
                Speciality = specialities[0],
                SpecialityId = specialities[0].Id,
                User = users[2]
            },

        };

        var clients = new List<Client>
        {
            new Client
            {
                Id = Guid.Parse("123e4567-e89b-12d3-a456-426614174008"),
                UserId = users[3].Id,
                User = users[3]
            },
            new Client
            {
                Id = Guid.Parse("123e4567-e89b-12d3-a456-426614174015"),
                UserId = users[4].Id,
                User = users[4]
            },
            new Client
            {
                Id = Guid.NewGuid(),
                UserId = users[5].Id,
                User = users[5]
            }
        };

        var availabilities = new List<Availability>
        {
            new Availability
            {
                Id = Guid.Parse("223e4567-e89b-12d3-a456-426614174000"),
                DoctorId = doctors[1].Id,
                SlotDateTime = DateTime.UtcNow.AddDays(1).AddHours(9),
                DurationMinutes = 15,
                IsBooked = false
            },
            new Availability
            {
                Id = Guid.NewGuid(),
                DoctorId = doctors[1].Id,
                SlotDateTime = DateTime.UtcNow.AddDays(2).AddHours(10),
                DurationMinutes = 15,
                IsBooked = false
            },
            new Availability
            {
                Id = Guid.NewGuid(),
                DoctorId = doctors[1].Id,
                SlotDateTime = DateTime.UtcNow.AddDays(3).AddHours(8),
                DurationMinutes = 15,
                IsBooked = false
            }
        };

        var appointments = new List<Appointment>
        {
            new Appointment
            {
                Id = Guid.NewGuid(),
                ClientId = clients[1].Id,
                DoctorId = doctors[2].Id,
                AvailabilityId = availabilities[1].Id,
                AppointmentDateTime = availabilities[1].SlotDateTime,
                AppointmentStatus = Domain.Enum.AppointmentStatus.Scheduled,
                Notes = "Initial consultation",
                Availability = availabilities[1],
                Client = clients[1],
                Doctor = doctors[2]
            },
            new Appointment 
            {
                Id = Guid.Parse("323e4567-e89b-12d3-a456-426614174025"),
                ClientId = clients[0].Id,
                DoctorId = doctors[1].Id,
                AvailabilityId = availabilities[0].Id,
                AppointmentDateTime = availabilities[0].SlotDateTime,
                AppointmentStatus = Domain.Enum.AppointmentStatus.Scheduled,
                Notes = "Initial consultation",
                Availability = availabilities[0],
                Client = clients[0],
                Doctor = doctors[1]
            }
        };

        var userRoles = new List<UserRole>
        {
            new UserRole { UserId = users[0].Id, RoleId = Roles[1].Id },
            new UserRole { UserId = users[1].Id, RoleId = Roles[1].Id }, 
            new UserRole { UserId = users[1].Id, RoleId = Roles[0].Id }, 
            new UserRole { UserId = users[2].Id, RoleId = Roles[1].Id },
            new UserRole { UserId = users[3].Id, RoleId = Roles[2].Id },
            new UserRole { UserId = users[4].Id, RoleId = Roles[2].Id },
            new UserRole { UserId = users[5].Id, RoleId = Roles[2].Id } 
        };

        context.Roles.AddRange(Roles);
        context.Specialities.AddRange(specialities);
        context.Users.AddRange(users);
        context.UserRoles.AddRange(userRoles);
        context.Doctors.AddRange(doctors);
        context.Clients.AddRange(clients);
        context.Availabilities.AddRange(availabilities);
        context.Appointments.AddRange(appointments);
        context.SaveChanges();
    }
}
