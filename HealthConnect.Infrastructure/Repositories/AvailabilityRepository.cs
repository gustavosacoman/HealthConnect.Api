namespace HealthConnect.Infrastructure.Repositories;

using HealthConnect.Application.Dtos.Availability;
using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Domain.Models;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

public class AvailabilityRepository(
    AppDbContext appDbContext,
    IMapper mapper) : IAvailabilityRepository
{
    private readonly AppDbContext _appDbContext = appDbContext;
    private readonly IMapper _mapper = mapper;

    public async Task CreateAvailabilityAsync(Availability availability)
    {
        await _appDbContext.Availabilities.AddAsync(availability);
    }

    public async Task<bool> HasOverlappingAvailabilityAsync(Guid doctorId, DateTime newSlotStart, DateTime newSlotEnd)
    {
        return await _appDbContext.Availabilities
            .AnyAsync(a =>
            a.DoctorId == doctorId &&
                a.SlotDateTime < newSlotEnd &&
                (a.SlotDateTime.AddMinutes(a.DurationMinutes) > newSlotStart));
    }

    public async Task<IEnumerable<TProjection>> GetAllAvailabilityPerDoctor<TProjection>(Guid doctorId)
    {
        return await _appDbContext.Availabilities
            .Where(a => a.DoctorId == doctorId)
            .ProjectTo<TProjection>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<Availability> GetAvailabilityByIdAsync(Guid id)
    {
        return await _appDbContext.Availabilities.FindAsync(id);
    }
}
