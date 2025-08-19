using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthConnect.Api.Tests;

public class GlobalExceptionHandlerMiddlewareTests
{

    [Fact]
    public async Task Middleware_WhenKeyNotFoundExceptionIsThrown_ReturnsNotFoundResponse()
    {
        var faultyFactory = new FaultyApiWebAppFactory();

        faultyFactory.ConfigurePipeline = app =>
        {
            app.UseMiddleware<HealthConnect.Api.Middleweres.GlobalExceptionHandlerMiddleware>();
            app.Run(context => throw new KeyNotFoundException("test resources not found"));
        };

        var faultyClient = faultyFactory.CreateClient();

        var response = await faultyClient.GetAsync("/whatever-route-will-fail");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var errorResponse = JsonDocument.Parse(content).RootElement;

        Assert.Equal("application/json", response.Content.Headers.ContentType?.ToString());
        Assert.Equal(404, errorResponse.GetProperty("StatusCode").GetInt32());
        Assert.Equal("test resources not found", errorResponse.GetProperty("Message").GetString());
    }
}
