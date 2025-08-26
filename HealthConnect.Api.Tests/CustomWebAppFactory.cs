using HealthConnect.Application;
using HealthConnect.Infrastructure;
using HealthConnect.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace HealthConnect.Api;

public class CustomWebAppFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"InMemoryDbForTesting-{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, conf) =>
        {
            conf.AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Jwt:Key"] = "MySecretKeyForTestsWith34BytesThatNoOneCanSee",
                ["Jwt:Issuer"] = "IssuerTest",
                ["Jwt:Audience"] = "AudienceTest",
            });
        });

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase(_databaseName);
            });

            services.AddControllers()
                .AddApplicationPart(typeof(Controllers.v1.AuthController).Assembly);
            services.AddApplication();
            services.AddRepositories();
        });
        builder.Configure(app =>
        {
            app.UseMiddleware<Middleweres.GlobalExceptionHandlerMiddleware>();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        });

    }
}