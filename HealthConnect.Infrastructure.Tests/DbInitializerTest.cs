using HealthConnect.Application.Interfaces.ServicesInterface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HealthConnect.Application;

namespace HealthConnect.Infrastructure.Tests;

public class DbInitializerTest
{
    [Fact]
    public async Task CreateInitializerAsync_ShouldCreateAdminUsers_WhenTheyDoNotExist()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddDbContext<Data.AppDbContext>(options =>
            options.UseInMemoryDatabase("TestDb"));

        serviceCollection.AddLogging();
        serviceCollection.AddApplication();
        serviceCollection.AddRepositories();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        await DbInitializer.InitializeAsync(serviceProvider);

        var context = serviceProvider.GetRequiredService<Data.AppDbContext>();

        Assert.True(await context.Users.AnyAsync(u => u.Email == "julia@admin.com"));
        Assert.True(await context.Users.AnyAsync(u => u.Email == "john@admin.com"));

    }
}
