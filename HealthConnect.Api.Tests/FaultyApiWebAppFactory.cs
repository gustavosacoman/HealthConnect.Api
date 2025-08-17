using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Api.Tests;

public class FaultyApiWebAppFactory : WebApplicationFactory<Program>

{
    public Action<IApplicationBuilder> ConfigurePipeline { get; set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.Configure(app =>
        {
            if (ConfigurePipeline != null)
            {
                ConfigurePipeline(app);
            }
        });
    }

}
