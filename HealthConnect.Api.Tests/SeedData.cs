using HealthConnect.Application.Interfaces;
using HealthConnect.Domain.Enum;
using HealthConnect.Domain.Models;
using HealthConnect.Domain.Models.Roles;
using HealthConnect.Domain.Models.Specialities;
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
                Sex = Sex.Female,
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
                Sex = Sex.Male,
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
                Sex = Sex.Female,
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
                Sex = Sex.Female,
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
                Sex = Sex.Male,
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
                Sex = Sex.Female,
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
                RQE = "654321",
                User = users[0]
            },
            new Doctor
            {
                Id = Guid.Parse("123e4567-e89b-12d3-a456-426614174001"),
                UserId = users[1].Id,
                RQE = "987654",
                User = users[1]

            },
            new Doctor
            {
                Id = Guid.Parse("123e4567-e89b-12d3-a456-426614174010"),
                UserId = users[2].Id,
                RQE = "210987",
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

        var doctorSpeciality = new List<DoctorSpeciality>
        { 
            new DoctorSpeciality
            {
                DoctorId = doctors[0].Id,
                SpecialityId = specialities[0].Id,
                RqeNumber = "546084",
                Doctor = doctors[0],
                Speciality = specialities[0]

            },
            new DoctorSpeciality
            {
                DoctorId = doctors[1].Id,
                SpecialityId = specialities[1].Id,
                RqeNumber = "987654",
                Doctor = doctors[1],
                Speciality = specialities[1]

            },
            new DoctorSpeciality
            {
                DoctorId = doctors[2].Id,
                SpecialityId = specialities[2].Id,
                RqeNumber = "987655",
                Doctor = doctors[2],
                Speciality = specialities[2]

            },
        };

        var userRoles = new List<UserRole>
        {
            new UserRole { Role = Roles[1], User = users[0] },
            new UserRole {Role = Roles[1], User = users[1]}, 
            new UserRole {Role = Roles[0], User = users[1]}, 
            new UserRole {Role = Roles[1], User = users[2]},
            new UserRole {Role = Roles[2], User = users[3]},
            new UserRole {Role = Roles[2], User = users[4]},
            new UserRole {Role = Roles[2], User = users[5]} 
        };

        var CRMs = new List<DoctorCRM> 
        {
            new DoctorCRM { Id = Guid.Parse("323e4567-e89b-12d3-a456-426614174825"),
                CRMNumber = "123456", State = "PR", Doctor = doctors[0], DoctorId = doctors[0].Id },
            new DoctorCRM { Id = Guid.NewGuid(), CRMNumber = "654321", State = "PR", Doctor = doctors[1], DoctorId = doctors[1].Id },
            new DoctorCRM { Id = Guid.NewGuid(), CRMNumber = "789012", State = "PR", Doctor = doctors[2], DoctorId = doctors[1].Id },
        };

        var doctorOffice = new List<DoctorOffice>
        {
            new DoctorOffice
            {
                Id = Guid.Parse("423e4567-e89b-12d3-a456-426614174555"),
                DoctorId = doctors[1].Id,
                Street = "Main St",
                Number = 123,
                Complement = "Suite 1",
                Neighborhood = "Downtown",
                City = "Some City",
                State = "PR",
                ZipCode = "12345-678",
                Phone = "123-456-7890",
                Doctor = doctors[1],
                IsPrimary = true
            },
            new DoctorOffice
            {
                Id = Guid.NewGuid(),
                DoctorId = doctors[1].Id,
                Street = "Second St",
                Number = 456,
                Complement = "Suite 2",
                Neighborhood = "Downtown",
                City = "Some City",
                State = "PR",
                ZipCode = "23456-789",
                Phone = "234-567-8901",
                Doctor = doctors[1],
                IsPrimary = false
            }
        };
        
        context.Roles.AddRange(Roles);
        context.Specialities.AddRange(specialities);
        context.Users.AddRange(users);
        context.UserRoles.AddRange(userRoles);
        context.Doctors.AddRange(doctors);
        context.DoctorCRMs.AddRange(CRMs);
        context.DoctorSpecialities.AddRange(doctorSpeciality);
        context.Clients.AddRange(clients);
        context.Availabilities.AddRange(availabilities);
        context.Appointments.AddRange(appointments);
        context.DoctorOffices.AddRange(doctorOffice);
        context.SaveChanges();
    }
}
