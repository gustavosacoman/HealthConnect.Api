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
public class AppointmentRepository(
    AppDbContext appDbContext,
    IMapper mapper)
    : IAppointmentRepository
{
    private readonly AppDbContext _appDbConxtext = appDbContext;
    private readonly IMapper _mapper = mapper;

    /// <inheritdoc/>
    public async Task CreateAppointmentAsync(Appointment appointment)
    {
       await _appDbConxtext.Appointments.AddAsync(appointment);
    }

    /// <inheritdoc/>
    public async Task<Appointment?> GetAppointmentByClientId(Guid clientId)
    {
        return await _appDbConxtext.Appointments
            .Include(a => a.Client)
            .Include(a => a.Doctor)
            .FirstOrDefaultAsync(a => a.ClientId == clientId);
    }

    /// <inheritdoc/>
    public async Task<Appointment?> GetAppointmentByIdAsync(Guid id)
    {
        return await _appDbConxtext.Appointments.FindAsync(id);
    }

    /// <inheritdoc/>
    public async Task<TProjection?> GetAppointmentByIdQueryAsync<TProjection>(Guid id)
    {
        return await _appDbConxtext.Appointments
            .Where(a => a.Id == id)
            .ProjectTo<TProjection>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public IQueryable<Appointment> GetAppointmentByDoctorIdQueryAsync(Guid doctorId)
    {
        return _appDbConxtext.Appointments.AsNoTracking();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Appointment>> GetAppointmentsByClientIdAsync(Guid clientId)
    {
        return await _appDbConxtext.Appointments
            .Include(a => a.Doctor)
            .ThenInclude(d => d.User)
            .Include(a => a.Client)
            .ThenInclude(c => c.User)
            .Include(a => a.Availability)
            .Where(a => a.ClientId == clientId).ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(Guid doctorId)
    {
        return await _appDbConxtext.Appointments
        .Include(a => a.Doctor)
            .ThenInclude(d => d.User)
        .Include(a => a.Client)
            .ThenInclude(c => c.User)
        .Include(a => a.Availability)
        .Where(a => a.DoctorId == doctorId)
        .ToListAsync();
    }
}
