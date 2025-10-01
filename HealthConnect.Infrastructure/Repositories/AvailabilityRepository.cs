namespace HealthConnect.Infrastructure.Repositories;

using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Domain.Models;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

/// <summary>
/// Repository for managing avaiability.
/// </summary>
public class AvailabilityRepository(
    AppDbContext appDbContext,
    IMapper mapper)
    : IAvailabilityRepository
{
    private readonly AppDbContext _appDbContext = appDbContext;
    private readonly IMapper _mapper = mapper;

    /// <inheritdoc/>
    public async Task CreateAvailabilityAsync(Availability availability)
    {
        await _appDbContext.Availabilities.AddAsync(availability);
    }

    /// <inheritdoc/>
    public async Task<bool> HasOverlappingAvailabilityAsync(Guid doctorId, DateTime newSlotStart, DateTime newSlotEnd)
    {
        return await _appDbContext.Availabilities
            .AnyAsync(a =>
            a.DoctorId == doctorId &&
                a.SlotDateTime < newSlotEnd &&
                (a.SlotDateTime.AddMinutes(a.DurationMinutes) > newSlotStart));
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TProjection>> GetAllAvailabilityPerDoctor<TProjection>(Guid doctorId)
    {
        return await _appDbContext.Availabilities
            .Where(a => a.DoctorId == doctorId)
            .ProjectTo<TProjection>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Availability?> GetAvailabilityByIdAsync(Guid id)
    {
        return await _appDbContext.Availabilities
            .Include(a => a.Doctor)
                .ThenInclude(d => d!.DoctorSpecialities)
                .ThenInclude(ds => ds.Speciality)
            .Include(a => a.DoctorOffice)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    /// <inheritdoc/>
    public Task DeleteAvailability(Availability availability)
    {
        _appDbContext.Availabilities.Remove(availability);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task<Availability?> GetAvailabilityByDoctorIdDateAscyn(Guid doctorId, DateTime slotDateTime)
    {
        return _appDbContext.Availabilities.Where(a => a.DoctorId == doctorId && a.SlotDateTime == slotDateTime)
            .OrderBy(a => a.SlotDateTime)
            .Include(a => a.Doctor)
                .ThenInclude(d => d!.DoctorSpecialities)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task CreateMultipleAvailabilitiesAsync(IEnumerable<Availability> availabilities)
    {
        await _appDbContext.Availabilities.AddRangeAsync(availabilities);
    }
}
