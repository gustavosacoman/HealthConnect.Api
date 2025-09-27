namespace HealthConnect.Infrastructure;

using HealthConnect.Application.Interfaces;
using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Infrastructure.Configurations;
using HealthConnect.Infrastructure.Data;
using HealthConnect.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Provides extension methods for registering infrastructure services in the dependency injection container.
/// </summary>
[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    /// <summary>
    /// Adds infrastructure services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when the connection string 'DefaultConnection' is not configured.</exception>
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Connection string 'DefaultConnection' is not configured.");
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }

    /// <summary>
    /// Adds repository services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add repository services to.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddTransient<IPasswordHasher, CryptoHelper>();

        services.AddScoped<IClientRepository, ClientRepository>();

        services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();

        services.AddScoped<IDoctorRepository, DoctorRepository>();

        services.AddScoped<IDoctorCRMRepository, DoctorCRMRepository>();

        services.AddScoped<IAppointmentRepository, AppointmentRepository>();

        services.AddScoped<ISpecialityRepository, SpecialityRepository>();

        services.AddScoped<IRoleRepository, RoleRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}