using HealthConnect.Domain.Models;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Api.Tests;

public static class SeedData
{
    public static void PopulateDatabase(AppDbContext context)
    {
        if (!context.Users.Any())
        {
            context.Users.RemoveRange(context.Users);
            context.SaveChanges();

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
                HashedPassword = "hashed_password_2",
                Salt = "another random_salt_value",
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
            }
        };
            context.Users.AddRange(users);
            context.SaveChanges();

        }
    }
}
