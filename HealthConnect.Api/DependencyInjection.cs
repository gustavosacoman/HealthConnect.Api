namespace HealthConnect.Api;

using HealthConnect.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using System.Net;
using System.Reflection;
using System.Text;
using FluentValidation.AspNetCore;

/// <summary>
/// Provides extension methods for registering and configuring presentation layer services.
/// </summary>
[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    /// <summary>
    /// Adds presentation layer services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {

        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
        });

        services.AddControllers()
        .AddXmlSerializerFormatters();

        services.AddFluentValidationAutoValidation();

        services.AddAuthorization();
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "HealthConnect API",
                Description = "API para o sistema de agendamento de consultas HealthConnect.",
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Insira o token JWT aqui, prefixado com 'Bearer '.",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                  new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                      Type = ReferenceType.SecurityScheme,
                      Id = "Bearer",
                    },
                },
                  Array.Empty<string>()
                },
            });
        });

        services.AddCors(options =>
        {
            options.AddPolicy(
                "AllowWebApp",
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

    /// <summary>
    /// Configures the presentation layer for the specified <see cref="WebApplication"/>.
    /// </summary>
    /// <param name="app">The web application to configure.</param>
    /// <returns>The configured web application.</returns>
    public static WebApplication UsePresentation(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var DbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            if (DbContext.Database.IsRelational())
            {
                DbContext.Database.Migrate();
            }
        }

        app.UseForwardedHeaders();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseAuthentication();
        app.UseHttpsRedirection();
        app.UseCors("AllowWebApp");
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }

    public static IServiceCollection AddJWTConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
            };
        });
        return services;
    }
}