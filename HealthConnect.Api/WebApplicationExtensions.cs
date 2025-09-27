using System;

/// <summary>
/// Provides extension methods for the <see cref="WebApplication"/> class.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Seeds the database using the <see cref="DbInitializer"/>.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            try
            {
                await DbInitializer.InitializeAsync(serviceProvider);
            }
            catch (Exception ex)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
    }
}
