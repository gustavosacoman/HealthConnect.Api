using HealthConnect.Api;
using HealthConnect.Api.Middleweres;
using HealthConnect.Application;
using HealthConnect.Infrastructure;
using System.Diagnostics.CodeAnalysis;

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
if (!app.Environment.IsEnvironment("Testing"))
{
    await app.SeedDatabaseAsync();
}
app.UsePresentation();
app.Run();

/// <summary>
/// Entry point for the HealthConnect API application.
/// </summary>
[ExcludeFromCodeCoverage]
public partial class Program
{
}