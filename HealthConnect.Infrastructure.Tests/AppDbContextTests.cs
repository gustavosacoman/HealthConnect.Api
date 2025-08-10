using HealthConnect.Domain.Models;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthConnect.Infrastructure.Tests;

public class AppDbContextTests
{
    [Fact]
    public async Task SaveChangesAsync_ShouldSetCreateAtAndUpdateAt_WhenAddingNewEntity()
    {

        var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
           .UseInMemoryDatabase(databaseName: "HealthConnectTestDb")
           .Options;

        await using var dbContext = new AppDbContext(dbContextOptions);

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Test User",
            Email = "teste.user@gmail.com",
            CPF = "12345678901",
            Phone = "123456789",
            HashedPassword = "password123",
            Salt = "randomSalt",
            BirthDate = new DateOnly(1990, 1, 1),

        };

        dbContext.Add(newUser);
        await dbContext.SaveChangesAsync();

        Assert.NotEqual(default(DateTime), newUser.CreatedAt);
        Assert.NotEqual(default(DateTime), newUser.UpdatedAt);

        Assert.Equal(newUser.CreatedAt, newUser.UpdatedAt);

        var timeDifference = DateTime.UtcNow - newUser.CreatedAt;
        Assert.True(timeDifference.TotalSeconds < 5, "Creation data is not recent.");

    }
    
}
