using System.Diagnostics.CodeAnalysis;
using HealthConnect.Api;
using HealthConnect.Application;
using HealthConnect.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation()
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

var app = builder.Build();

app.UsePresentation();

app.Run();

/// <summary>
/// Entry point for the HealthConnect API application.
/// </summary>
[ExcludeFromCodeCoverage]
public partial class Program
{
}