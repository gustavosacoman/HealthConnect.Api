using System.Diagnostics.CodeAnalysis;
using HealthConnect.Api;
using HealthConnect.Api.Middleweres;
using HealthConnect.Application;
using HealthConnect.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDatabase(builder.Configuration);
}

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddRepositories()
    .AddJWTConfiguration(builder.Configuration);

var app = builder.Build();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UsePresentation();
if (!app.Environment.IsEnvironment("Testing"))
{
    await app.SeedDatabaseAsync();
}

app.Run();

/// <summary>
/// Entry point for the HealthConnect API application.
/// </summary>
[ExcludeFromCodeCoverage]
public partial class Program
{
}