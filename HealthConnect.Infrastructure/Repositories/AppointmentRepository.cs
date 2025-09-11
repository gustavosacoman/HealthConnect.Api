namespace HealthConnect.Infrastructure.Repositories;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Domain.Models;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class AppointmentRepository(AppDbContext appDbContext, IMapper mapper) : IAppointmentRepository
{
    private readonly AppDbContext _appDbConxtext = appDbContext;
    private readonly IMapper _mapper = mapper;

    public async Task CreateAppointmentAsync(Appointment appointment)
    {
       await _appDbConxtext.Appointments.AddAsync(appointment);
    }

    public Task<Appointment?> GetAppointmentByClientId(Guid clientId)
    {
        throw new NotImplementedException();
    }

    public async Task<Appointment> GetAppointmentByIdAsync(Guid id)
    {
        return await _appDbConxtext.Appointments.FindAsync(id);
    }

    public async Task<TProjection> GetAppointmentByIdQueryAsync<TProjection>(Guid id)
    {
        return await _appDbConxtext.Appointments
            .Where(a => a.Id == id)
            .ProjectTo<TProjection>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsByClientIdAsync(Guid clientId)
    {
        return await _appDbConxtext.Appointments
            .Where(a => a.ClientId == clientId).ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(Guid doctorId)
    {
        return await _appDbConxtext.Appointments
            .Where(a => a.DoctorId == doctorId).ToListAsync();
    }
}
