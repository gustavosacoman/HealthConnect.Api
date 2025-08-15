using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.HttpOverrides;

namespace HealthConnect.Api;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
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