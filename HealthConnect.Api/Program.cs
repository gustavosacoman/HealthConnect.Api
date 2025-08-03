using HealthConnect.Api;
using HealthConnect.Infrastructure;
using System.Diagnostics.CodeAnalysis;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UsePresentation();

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program { }