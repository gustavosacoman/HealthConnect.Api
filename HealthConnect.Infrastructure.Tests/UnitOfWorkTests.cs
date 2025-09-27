using HealthConnect.Domain.Enum;
using HealthConnect.Domain.Models;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Infrastructure.Tests;

public class UnitOfWorkTests
{
    [Fact]
    public async Task SaveChangesAsync_ShouldPersistChangesToDatabase()
    {

        var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "UoW_TestDb_ShouldPersistChanges")
            .Options;

        await using var dbContext = new AppDbContext(dbContextOptions);

        await using var unitOfWork = new Repositories.UnitOfWork(dbContext);

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Test User",
            Email = "teste.user@gmail.com",
            CPF = "12345678901",
            Phone = "123456789",
            HashedPassword = "password123",
            Salt = "randomSalt",
            Sex = Sex.Male,
            BirthDate = new DateOnly(1990, 1, 1),

        };

        dbContext.Users.Add(newUser);

        await unitOfWork.SaveChangesAsync();

        await using var assertDbContext = new AppDbContext(dbContextOptions);

        var savedUser = await assertDbContext.Users.FirstOrDefaultAsync(u => u.Email == "teste.user@gmail.com");

        Assert.NotNull(savedUser);
        Assert.Equal(newUser.Email, savedUser.Email);

    }
}
