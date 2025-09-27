namespace HealthConnect.Infrastructure.Repositories;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Domain.Models;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Repository for managing Appointment entities in the database.
/// </summary>
/// <param name="appDbContext">Attribute to access the database.</param>
/// <param name="mapper">attribute to mapper configratuion.</param>
public class AppointmentRepository(AppDbContext appDbContext, IMapper mapper) : IAppointmentRepository
{
    private readonly AppDbContext _appDbConxtext = appDbContext;
    private readonly IMapper _mapper = mapper;

    /// <inheritdoc/>
    public async Task CreateAppointmentAsync(Appointment appointment)
    {
       await _appDbConxtext.Appointments.AddAsync(appointment);
    }

    /// <inheritdoc/>
    public Task<Appointment?> GetAppointmentByClientId(Guid clientId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<Appointment> GetAppointmentByIdAsync(Guid id)
    {
        return await _appDbConxtext.Appointments.FindAsync(id);
    }

    /// <inheritdoc/>
    public async Task<TProjection> GetAppointmentByIdQueryAsync<TProjection>(Guid id)
    {
#pragma warning disable CS8603
        return await _appDbConxtext.Appointments
            .Where(a => a.Id == id)
            .ProjectTo<TProjection>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
#pragma warning restore CS8603
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Appointment>> GetAppointmentsByClientIdAsync(Guid clientId)
    {
        return await _appDbConxtext.Appointments
            .Where(a => a.ClientId == clientId).ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(Guid doctorId)
    {
        return await _appDbConxtext.Appointments
            .Where(a => a.DoctorId == doctorId).ToListAsync();
    }
}
