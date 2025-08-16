using HealthConnect.Api;
using HealthConnect.Api.Middleweres;
using HealthConnect.Application;
using HealthConnect.Infrastructure;
using System.Diagnostics.CodeAnalysis;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation()
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

var app = builder.Build();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UsePresentation();
app.Run();

/// <summary>
/// Entry point for the HealthConnect API application.
/// </summary>
[ExcludeFromCodeCoverage]
public partial class Program
{
}