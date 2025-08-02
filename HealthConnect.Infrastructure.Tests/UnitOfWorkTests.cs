using HealthConnect.Domain.Models;
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

        var newUser = new User { Email = "test@example.com" };

        dbContext.Users.Add(newUser);

        await unitOfWork.SaveChangesAsync();

        await using var assertDbContext = new AppDbContext(dbContextOptions);

        var savedUser = await assertDbContext.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");

        Assert.NotNull(savedUser);
        Assert.Equal(newUser.Email, savedUser.Email);

    }
}
