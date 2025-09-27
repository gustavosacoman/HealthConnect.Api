using HealthConnect.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Api.Tests;

public class FaultyApiWebAppFactory : WebApplicationFactory<Program>

{
    public Action<IApplicationBuilder>? ConfigurePipeline { get; set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

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
                options.UseInMemoryDatabase($"InMemoryDbForFaultyTest-{Guid.NewGuid()}");
            });

        });

        builder.Configure(app =>
        {
            if (ConfigurePipeline != null)
            {
                ConfigurePipeline(app);
            }
        });
    }

}
