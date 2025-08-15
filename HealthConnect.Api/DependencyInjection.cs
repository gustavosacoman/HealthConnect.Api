namespace HealthConnect.Api;

using System.Diagnostics.CodeAnalysis;
using HealthConnect.Infrastructure.Data;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Provides extension methods for registering and configuring presentation layer services.
/// </summary>
[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    /// <summary>
    /// Adds presentation layer services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowWebApp",
                policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
        });

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });

        return services;
    }

    /// <summary>
    /// Configures the presentation layer for the specified <see cref="WebApplication"/>.
    /// </summary>
    /// <param name="app">The web application to configure.</param>
    /// <returns>The configured web application.</returns>
    public static WebApplication UsePresentation(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var DbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            DbContext.Database.Migrate();
        }

        app.UseForwardedHeaders();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseCors("AllowWebApp");
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}