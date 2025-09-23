namespace HealthConnect.Application;


using FluentValidation;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Application.Mappers;
using HealthConnect.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

/// <summary>
/// Provides extension methods for registering application services and validators.
/// </summary>
[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    /// <summary>
    /// Registers application services and validators with the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddAutoMapperConfiguration();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAvailabilityService, AvailabilityService>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<ISpecialityService, SpecialityService>();
        services.AddScoped<IDoctorCRMService, DoctorCRMService>();
        services.AddScoped<IDoctorService, DoctorService>();
        
        return services;
    }

    /// <summary>
    /// Explicitly adds all AutoMapper profiles.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<ClientMapper>();
            cfg.AddProfile<DoctorMapper>();
            cfg.AddProfile<UserMapper>();
            cfg.AddProfile<AvailabilityMapper>();
            cfg.AddProfile<AppointmentMapper>();
            cfg.AddProfile<SpecialityMapper>();
            cfg.AddProfile<RoleMapper>();
        });
        return services;
    }
}
