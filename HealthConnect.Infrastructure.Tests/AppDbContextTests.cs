using HealthConnect.Domain.Models;
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

        var newUser = new User { Email = "test@example.com" };

        dbContext.Add(newUser);
        await dbContext.SaveChangesAsync();

        Assert.NotEqual(default(DateTime), newUser.CreatedAt);
        Assert.NotEqual(default(DateTime), newUser.UpdatedAt);

        Assert.Equal(newUser.CreatedAt, newUser.UpdatedAt);

        var timeDifference = DateTime.UtcNow - newUser.CreatedAt;
        Assert.True(timeDifference.TotalSeconds < 5, "Creation data is not recent.");



    }
    
}
